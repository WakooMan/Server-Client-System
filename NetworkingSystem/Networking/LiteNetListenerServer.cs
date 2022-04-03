using LiteNetLib;
using System;
using System.Net;
using System.Net.Sockets;

namespace Networking
{
    public class LiteNetListenerServer<TBaseMessageType> : INetEventListener where TBaseMessageType : class
    {
        private readonly IServer Server;
        private readonly Action<INetworkChannel> OnClientConnectedInvokeFunction;
        private readonly Action<INetworkChannel> OnClientDisconnectedInvokeFunction;
        public LiteNetListenerServer(IServer server, Action<INetworkChannel> _OnClientConnectedInvokeFunction, Action<INetworkChannel> _OnClientDisconnectedInvokeFunction)
        {
            Server = server;
            OnClientConnectedInvokeFunction = _OnClientConnectedInvokeFunction;
            OnClientDisconnectedInvokeFunction = _OnClientDisconnectedInvokeFunction;
        }
        public void OnConnectionRequest(ConnectionRequest request)
        {
            if (!Server.CanPlayerJoin())
            {
                request.Reject();
            }
            else
            {
                request.Accept();
            }
        }

        public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
        {
        }

        public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
        {
            peer.GetConnection().Latency = latency;
        }

        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            if (!reader.IsNull)
            {
                peer.GetConnection().Receive(reader.GetRemainingBytes());
            }
        }

        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
        {
        }

        public void OnPeerConnected(NetPeer peer)
        {
            NetworkChannel<TBaseMessageType> con = new NetworkChannel<TBaseMessageType>();
            con.SetPeer(peer);
            peer.Tag = con;
            Server.Connected(con);
            OnClientConnectedInvokeFunction(con);
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            OnClientDisconnectedInvokeFunction(peer.GetConnection());
            peer.GetConnection().Close(); 
        }
    }
}
