using LiteNetLib;
using System;
using System.Threading;

namespace Networking
{
    public abstract class ClientBase<TBaseMessageType> : IClient
        where TBaseMessageType: class
    {
        protected INetworkChannel Channel;
        private readonly NetManager LanManager;
        private bool canConnect;
        protected DateTime LastSentKeepAliveMsg { get; set; } = DateTime.UtcNow;
        protected string IP;
        protected int Port;
        private event EventHandler OnConnected, OnDisconnected;
        protected Action<TBaseMessageType> DispatchMessage;
        public bool IsConnected { get => !Channel?.IsClosed??false; }
        public bool CanConnect { get => canConnect; }
        public bool TryingToConnect { get; private set; } = false;

        protected ClientBase(Action<TBaseMessageType> dispatchMessage)
        {
            canConnect = false;
            DispatchMessage = dispatchMessage;
            OnConnected += (s, e) => OnConnectedEvent((INetworkChannel)s);
            OnDisconnected += (s, e) => OnDisconnectedEvent();
            LanManager = new NetManager(new LiteNetListenerClient<TBaseMessageType>(this, (Channel) => OnConnected?.Invoke(Channel, null), () => OnDisconnected?.Invoke(this, null)));
        }
        public void ConnectTo(string IPAddr, int _port)
        {
            IP = IPAddr;
            Port = _port;
            LanManager.Start();
            canConnect = true;
        }

        public void Disconnect()
        {
            IP = "";
            Port = 0;
            canConnect = false;
            LanManager.Stop();
            Channel?.Close();
        }
        public void AddOnConnectedEvent(Action OnConnected) => this.OnConnected += (s, e) => OnConnected();
        public void AddOnDisconnectedEvent(Action OnDisconnected) => this.OnDisconnected += (s, e) => OnDisconnected();

        private void OnConnectedEvent(INetworkChannel channel)
        {
            Channel = channel;
            Bind(Channel);
        }

        private void Bind(INetworkChannel Channel)
         => Channel.OnMessage((INetworkChannel channel,TBaseMessageType msg)=>DispatchMessage(msg));

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

        protected void Send<T>(T msg,EDeliveryMethod EMethod) where T: class =>  Channel.Send(msg,EMethod);
        
        //This is called on Application Tick or in ClientLoop
        //Virtual, because there we should send the Data that is needed to send real time (for example: player movement in battle) in derived class.
        public virtual void Update()
        {
            if (!IsConnected && CanConnect)
            {
                if (!TryingToConnect)
                    TryConnectOnNewThread(-1, 1);
            }
            LanManager?.PollEvents();
        }

        public abstract void Send<T>(T msg) where T : class;
    }
}
