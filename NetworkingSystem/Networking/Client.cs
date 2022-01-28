using LiteNetLib;
using System;
using System.Threading;

namespace Networking
{
    public abstract class Client<TNetworkChannel,TProtocol,TMessageDispatcher,TMessageType>: IClient
        where TNetworkChannel : NetworkChannel<TProtocol, TMessageType>, new()
        where TProtocol : Protocol<TMessageType>, new()
        where TMessageDispatcher : MessageDispatcher<TMessageType>, new()
        where TMessageType : class, new()
    {
        protected INetworkChannel Channel;
        private readonly NetManager LanManager;
        protected readonly TMessageDispatcher MessageDispatcher;
        private DateTime LastSentKeepAliveMsg { get; set; } = DateTime.UtcNow;
        public ClientNetworkStateManager ClientStateManager { get; protected set; }
        protected string IP;
        protected int Port;
        public bool IsConnected { get => !Channel?.IsClosed??false; }
        public Client(string IPAddr, int _port)
        {
            IP = IPAddr;
            Port = _port;

            LanManager = new NetManager(new LiteNetListenerClient<TNetworkChannel,TProtocol,TMessageDispatcher,TMessageType>(this, SetNetworkChannelEventMethod));
            MessageDispatcher = new TMessageDispatcher();

            LanManager.Start();
        }

        public void TryConnectOnNewThread(int AttemptsNum, int SecondsBetweenAttempts) 
            => new Thread(()=> TryConnect(AttemptsNum, SecondsBetweenAttempts)).Start();
        //giving -1 as AttemptsNum results in an infinite loop.
        public void TryConnect(int AttemptsNum,int SecondsBetweenAttempts) 
        {
            int Count = 0;
            while (Count!=AttemptsNum&&!IsConnected)
            {
                LanManager.Connect(IP,Port,"valami");
                Count++;
                Thread.Sleep(SecondsBetweenAttempts * 1000);
            }
        }

        private void SetNetworkChannelEventMethod(INetworkChannel channel) 
        { 
            Channel = channel; 
            MessageDispatcher.Bind(Channel);
        }

        public void Bind<TController>() => MessageDispatcher.Bind<TController>();

        public void Disconnect() 
        { 
            Channel.Close();
            LanManager.Stop();
        }
        public void StartClientLoopOnNewThread()
         => new Thread(ClientLoop).Start();

        //This is the client loop,which has the logic about when and what to send to the server.
        protected abstract void ClientLoop();

        public void Send<T>(T msg) where T: Message =>  Channel.Send(msg,msg.EMethod);
        
        //This is called on Application Tick or in ClientLoop
        //Virtual, because there we should send the Data that is needed to send real time (for example: player movement in battle) in derived class.
        public virtual void Update()
        {
            if (DateTime.Compare(DateTime.UtcNow.Subtract(new TimeSpan(0, 0, 5)), LastSentKeepAliveMsg) > 0)
            {
                Send(new KeepAliveRequestMessage());
                LastSentKeepAliveMsg = DateTime.UtcNow;
            }
           LanManager?.PollEvents();
        }
    }
}
