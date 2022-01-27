using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using LiteNetLib;

namespace Networking
{
    public class LiteNetListenerClient<TNetworkChannel, TProtocol, TMessageDispatcher, TMessageType> : INetEventListener
        where TNetworkChannel : NetworkChannel<TProtocol, TMessageType>, new()
        where TProtocol : Protocol<TMessageType>, new()
        where TMessageDispatcher : MessageDispatcher<TMessageType>, new()
        where TMessageType : class, new()
    {
        private readonly IClient Client;
        Action<INetworkChannel> SetNetworkChannelMethod;
        public LiteNetListenerClient(IClient client,Action<INetworkChannel> method) 
        { 
            Client = client;
            SetNetworkChannelMethod = method;
        }
        public void OnConnectionRequest(ConnectionRequest request)
        {
        }

        public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
        {
        }

        public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
        {
        }

        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            if (reader.IsNull)
            {
                Console.WriteLine("The message is null.");
            }

            peer.GetConnection()?.Receive(reader.GetRemainingBytes());
        }

        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
        {
        }

        public void OnPeerConnected(NetPeer peer)
        {
            TNetworkChannel connection = new TNetworkChannel();
            connection.SetPeer(peer);
            peer.Tag = connection;
            SetNetworkChannelMethod(connection);
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            peer.Tag = null;
        }
    }
}
