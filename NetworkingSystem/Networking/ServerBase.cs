
using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Networking
{
    public abstract class ServerBase<TBaseMessageType>: IServer
        where TBaseMessageType : class
    {
        //State variable is initiated in child class
        protected readonly NetManager WanManager;
        protected readonly NetManager LanManager;
        private event EventHandler OnClientConnected, OnClientDisconnected;
        public readonly ChannelManager _channelManager;
        protected Action<INetworkChannel,TBaseMessageType> DispatchMessage;
        public bool Running = false;
        private int MaxClients;
        private int CurrentClients;
        public virtual bool CanPlayerJoin() => CurrentClients < MaxClients;
        protected ServerBase(Action<INetworkChannel, TBaseMessageType> dispatchMessage)
        {
            //WanManager = new NetManager(new LiteNetListenerServer(this,() => OnClientConnected?.Invoke(this,null), () => OnClientDisconnected?.Invoke(this,null)));
            LanManager = new NetManager(new LiteNetListenerServer<TBaseMessageType>(this,(Channel) => OnClientConnected?.Invoke(Channel, null), (Channel) => OnClientDisconnected?.Invoke(Channel, null)));
            MaxClients = 0;
            CurrentClients = 0;
            OnClientConnected += (s, e) => CurrentClients++;
            OnClientDisconnected += (s, e) => CurrentClients--;
            _channelManager = new ChannelManager();
            DispatchMessage = dispatchMessage;
        }
        public void Connected(INetworkChannel Channel) 
        {
            Bind(Channel);
            _channelManager.Accept(Channel);
        }
        public void AddOnClientConnectedEvent(Action<INetworkChannel> OnClientConnected) => this.OnClientConnected += (s, e) => OnClientConnected((INetworkChannel)s);
        public void AddOnClientConnectedEvent(Action OnClientConnected) => this.OnClientConnected += (s,e)=> OnClientConnected();
        public void AddOnClientDisconnectedEvent(Action OnClientDisconnected) => this.OnClientDisconnected += (s, e) => OnClientDisconnected();
        public void AddOnClientDisconnectedEvent(Action<INetworkChannel> OnClientDisconnected) => this.OnClientDisconnected += (s, e) => OnClientDisconnected((INetworkChannel)s);

        public void Start(int port,int maxclients)
        {
            Running = true;
            LanManager.Start(port);
            MaxClients = maxclients;
            //WanManager.Start(port);
        }

        public void Stop()
        {
            LanManager.Stop();
            //WanManager.Stop();
            Running = false;
        }

        protected void SendTo<T>(Guid Id, T Message, EDeliveryMethod EMethod) where T : class => _channelManager.SendTo(Id, Message,EMethod);

        protected void Broadcast<T>(Guid ExceptionId,T Message,EDeliveryMethod EMethod) where T : class => _channelManager.Broadcast(ExceptionId, Message,EMethod);
        protected void Multicast<T>(List<Guid> Group, T Message, EDeliveryMethod EMethod) where T : class => _channelManager.Multicast(Group, Message, EMethod);

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

        private void Bind(INetworkChannel Channel)
        => Channel.OnMessage(DispatchMessage);

        public abstract void SendTo<T>(Guid Id, T Message) where T : class;

        public abstract void Broadcast<T>(Guid ExceptionId, T Message) where T : class;
        public abstract void Multicast<T>(List<Guid> Group, T Message) where T : class;
    }
}
