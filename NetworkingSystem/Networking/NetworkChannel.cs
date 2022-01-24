using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Networking
{
    public class NetworkChannel<TProtocol, TMessageType> : IDisposable, INetworkChannel where TProtocol : Protocol<TMessageType>, new ()
    {
        private readonly NetPeer m_Peer;
        protected bool isDisposed = false;
        public bool IsClosed { get; protected set; } = false;
        protected readonly TProtocol protocol = new TProtocol();
        private Action<TMessageType> messageCallback;
        public event EventHandler Closed;
        public NetworkChannel(NetPeer peer) { m_Peer = peer; Latency = m_Peer.Ping; }
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
            messageCallback(msg);
        }

        public void Send<T>(T Message,EDeliveryMethod eMethod)
        {
            protocol.Send(m_Peer,eMethod,Message);
        }

        public void OnMessage(Action<TMessageType> p) => messageCallback = p;

        public void OnMessage<TMessageType1>(Action<TMessageType1> p) => OnMessage(p as Action<TMessageType>);
        
    }
}
