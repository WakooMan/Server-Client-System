using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Networking
{
    public interface IClientNetworkChannels<TUDPChannel,TTCPChannel,TProtocol,TMessageType> : INetworkChannels
        where TUDPChannel : UDPNetworkChannel<TProtocol,TMessageType>
        where TTCPChannel: ClientTCPChannel<TProtocol,TMessageType>
        where TProtocol : Protocol<TMessageType>, new()
    {
        Guid Id { get; }
        TUDPChannel UDPChannel { get; }
        TTCPChannel TCPChannel { get; }

        Task ConnectAsync(IPEndPoint EndPoint);
    }
}
