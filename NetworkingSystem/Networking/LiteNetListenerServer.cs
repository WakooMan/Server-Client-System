using LiteNetLib;
using System;
using System.Net;
using System.Net.Sockets;

namespace Networking
{
    public class LiteNetListenerServer : INetEventListener
    {
        private readonly Server Server;
        private readonly Action OnClientConnectedInvokeFunction;
        private readonly Action OnClientDisconnectedInvokeFunction;
        public LiteNetListenerServer(Server server, Action _OnClientConnectedInvokeFunction, Action _OnClientDisconnectedInvokeFunction)
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
                Console.WriteLine("Someone Tried to Connect, but connection got rejected.");
            }
            else
            {
                request.Accept();
                Console.WriteLine("Someone Tried to Connect. Connection Accepted");
                OnClientConnectedInvokeFunction();
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
            ObjectNetworkChannel con = new ObjectNetworkChannel(peer);
            peer.Tag = con;
            Server.Connected(con);
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            OnClientDisconnectedInvokeFunction();
            peer.GetConnection().Close(); 
        }
    }
}
