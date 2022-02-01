using LiteNetLib;
using Networking.Protobuf;
using System;
using System.Threading;

namespace Networking
{
    public abstract class Client<TBaseMessageType> : IClient
    {
        protected INetworkChannel Channel;
        private readonly NetManager LanManager;
        protected readonly ProtobufMsgDispatcher MessageDispatcher;
        protected DateTime LastSentKeepAliveMsg { get; set; } = DateTime.UtcNow;
        public ClientNetworkStateManager ClientStateManager { get; protected set; }
        protected string IP;
        protected int Port;
        private event EventHandler OnConnected, OnDisconnected;
        public bool IsConnected { get => !Channel?.IsClosed??false; }
        public bool TryingToConnect { get; private set; } = false;

        public Client(string IPAddr, int _port)
        {
            IP = IPAddr;
            Port = _port;
            OnConnected += (s, e) => OnConnectedEvent((INetworkChannel)s);
            OnDisconnected += (s, e) => OnDisconnectedEvent();
            LanManager = new NetManager(new LiteNetListenerClient<TBaseMessageType>(this, (Channel) => OnConnected?.Invoke(Channel, null), () => OnDisconnected?.Invoke(this, null)));
            MessageDispatcher = new ProtobufMsgDispatcher();
            ClientMessageHandler.Client = this;
            LanManager.Start();
        }

        public void AddOnConnectedEvent(Action OnConnected) => this.OnConnected += (s, e) => OnConnected();
        public void AddOnDisconnectedEvent(Action OnDisconnected) => this.OnDisconnected += (s, e) => OnDisconnected();

        private void OnConnectedEvent(INetworkChannel channel)
        {
            Channel = channel;
            MessageDispatcher.Bind(Channel);
        }

        private void OnDisconnectedEvent()
        {
            Channel?.Close();
            Channel = null;
        }

        public void TryConnectOnNewThread(int AttemptsNum, int SecondsBetweenAttempts) 
            => new Thread(()=> TryConnect(AttemptsNum, SecondsBetweenAttempts)).Start();
        //giving -1 as AttemptsNum results in an infinite loop.
        public void TryConnect(int AttemptsNum,int SecondsBetweenAttempts) 
        {
            int Count = 0;
            TryingToConnect = true;
            while (Count!=AttemptsNum&&!IsConnected)
            {
                LanManager.Connect(IP,Port,"valami");
                Count++;
                Thread.Sleep(SecondsBetweenAttempts * 1000);
            }
            TryingToConnect = false;
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
            if (!IsConnected)
            {
                if (!TryingToConnect)
                    TryConnectOnNewThread(-1, 1);
            }
           LanManager?.PollEvents();
        }
    }
}
