using LiteNetLib;
using System;

namespace Networking
{
    public class NetworkChannel<TBaseMessageType> : IDisposable, INetworkChannel
    {
        private NetPeer m_Peer;
        protected bool isDisposed = false;
        public bool IsClosed { get; protected set; } = false;
        protected readonly Protocol<TBaseMessageType> protocol = new Protocol<TBaseMessageType>();
        private Action<INetworkChannel,TBaseMessageType> messageCallback;
        public event EventHandler Closed;
        public NetworkChannel() { }
        public void SetPeer(NetPeer peer) => m_Peer = peer;
        public Guid Id { get; } = Guid.NewGuid();

        public int FragmentLength => 100_000;
        public int MaxPackageLength => 100_000_000;

        public int Latency { get; set; }

        public void Close()
        {
            if (!IsClosed)
            {
                IsClosed = true;
                m_Peer.NetManager.DisconnectPeer(m_Peer, new byte[] { });
                Closed?.Invoke(this,null);
            }
        }

        public void Dispose()
        {
            if (!isDisposed)
            {
                isDisposed = true;
                Close();
            }
        }

        public void Receive(byte [] bytes)
        {
            var msg = protocol.Receive(bytes);
            messageCallback(this,msg);
        }

        public void Send<T>(T Message,EDeliveryMethod eMethod) where T : class 
        {
            protocol.Send(m_Peer,eMethod,Message);
        }

        public void OnMessage(Action<INetworkChannel,TBaseMessageType> p) => messageCallback = p;

        public void OnMessage<TBaseMessageType1>(Action<INetworkChannel,TBaseMessageType1> p) => OnMessage(p as Action<INetworkChannel,TBaseMessageType>);
        
    }
}
