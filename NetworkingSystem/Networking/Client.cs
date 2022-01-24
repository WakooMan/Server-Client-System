using LiteNetLib;
using System;
using System.Threading;

namespace Networking
{
    public abstract class Client: IUpdatable
    {
        protected INetworkChannel Channel;
        private readonly NetManager LanManager;
        protected readonly ObjectMessageDispatcher MessageDispatcher;
        private DateTime LastSentKeepAliveMsg { get; set; } = DateTime.UtcNow;
        private int KeepAlivePacketID = 0;
        public ClientNetworkStateManager ClientStateManager { get; protected set; }
        protected string IP;
        protected int Port;
        public bool IsConnected { get => !Channel?.IsClosed??false; }
        public Client(string IPAddr, int _port)
        {
            IP = IPAddr;
            Port = _port;

            LanManager = new NetManager(new LiteNetListenerClient(this, SetNetworkChannelEventMethod));
            MessageDispatcher = new ObjectMessageDispatcher();

            LanManager.Start();
            LanManager.Connect(IP,Port,"SomethingKey");
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
                Send(new KeepAliveRequestMessage(KeepAlivePacketID++));
                LastSentKeepAliveMsg = DateTime.UtcNow;
            }
           LanManager?.PollEvents();
        }
    }
}
