using System;
using System.Net;
using System.Net.Sockets;
using LiteNetLib;
using Networking.Protobuf;

namespace Networking
{
    public class LiteNetListenerClient<TBaseMessageType> : INetEventListener
    {
        IClient Client;
        Action<INetworkChannel> OnConnectedInvokeMethod;
        Action OnDisconnectedInvokeMethod;
        public LiteNetListenerClient(IClient client,Action<INetworkChannel> OnConInvokemethod, Action OnDisconInvokeMethod)
        {
            Client = client;
            OnConnectedInvokeMethod = OnConInvokemethod;
            OnDisconnectedInvokeMethod = OnDisconInvokeMethod;
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
            }

            peer.GetConnection()?.Receive(reader.GetRemainingBytes());
        }

        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
        {
        }

        public void OnPeerConnected(NetPeer peer)
        {
            ProtobufChannel<TBaseMessageType> connection = new ProtobufChannel<TBaseMessageType>();
            connection.SetPeer(peer);
            peer.Tag = connection;
            OnConnectedInvokeMethod(connection);
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            OnDisconnectedInvokeMethod();
            peer.Tag = null;
        }
    }
}
