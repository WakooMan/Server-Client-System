
using LiteNetLib;
using Networking.Protobuf;
using System;
using System.Threading;

namespace Networking
{
    public abstract class Server<TBaseMessageType>: IServer
    {
        //State variable is initiated in child class
        public ServerNetworkStateManager ServerStateManager { get; protected set; }
        protected readonly NetManager WanManager;
        protected readonly NetManager LanManager;
        private event EventHandler OnClientConnected, OnClientDisconnected;
        public readonly ChannelManager _channelManager;
        protected readonly ProtobufMsgDispatcher _messageDispatcher = new ProtobufMsgDispatcher();
        public bool Running = false;
        private int MaxClients;
        private int CurrentClients;
        public bool CanPlayerJoin() => CurrentClients < MaxClients;
        public Server(int maxClients)
        {
            //WanManager = new NetManager(new LiteNetListenerServer(this,() => OnClientConnected?.Invoke(this,null), () => OnClientDisconnected?.Invoke(this,null)));
            LanManager = new NetManager(new LiteNetListenerServer<TBaseMessageType>(this,() => OnClientConnected?.Invoke(this, null), () => OnClientDisconnected?.Invoke(this, null)));
            ServerMessageHandler.Server = this;
            MaxClients = maxClients;
            CurrentClients = 0;
            OnClientConnected += (s, e) => CurrentClients++;
            _channelManager = new ChannelManager();
        }
        public void Connected(INetworkChannel Channel) 
        { 
            _channelManager.Accept(Channel);
            _messageDispatcher.Bind(Channel);
        }
        public void AddOnClientConnectedEvent(Action OnClientConnected) => this.OnClientConnected += (s,e)=> OnClientConnected();
        public void AddOnClientDisconnectedEvent(Action OnClientDisconnected) => this.OnClientDisconnected += (s, e) => OnClientDisconnected();

        

        public void Bind<TController>() => _messageDispatcher.Bind<TController>();

        public void Start(int port)
        {
            Running = true;
            LanManager.Start(port);
            //WanManager.Start(port);
        }

        public void Stop()
        {
            LanManager.Stop();
            //WanManager.Stop();
            Running = false;
        }

        public void SendTo<T>(Guid Id, T Message) where T : Message => _channelManager.SendTo(Id, Message, Message.EMethod);

        public void Broadcast<T>(Guid ExceptionId,T Message) where T : Message => _channelManager.Broadcast(ExceptionId, Message,Message.EMethod);

        public void StartServerLoopOnNewThread()
            => new Thread(ServerLoop).Start();

        public void StartServerLoop() => ServerLoop();

        //This is the server side loop, which has the logic about when to send message to a client.
        protected abstract void ServerLoop();

        //This is called in the application Tick, or in the server loop if the server is running.
        //Virtual, because there we should send the Data that is needed to send real time (for example: player movement in battle) in derived class.
        public virtual void Update()
        {
            if (Running)
            { 
                LanManager?.PollEvents();
                //WanManager?.PollEvents();
            }
        }
    }
}
