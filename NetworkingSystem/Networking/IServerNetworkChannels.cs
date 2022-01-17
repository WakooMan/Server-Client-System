using System;

namespace Networking
{
    public interface IServerNetworkChannels<TUDPChannel,TTCPChannel,TProtocol,TMessageType> : INetworkChannels
    where TUDPChannel : UDPNetworkChannel<TProtocol, TMessageType>
    where TTCPChannel : TCPNetworkChannel<TProtocol, TMessageType>
    where TProtocol : Protocol<TMessageType>, new()
    {
        Guid Id { get; }

        event EventHandler Closed;
        TUDPChannel UDPChannel { get; }
        TTCPChannel TCPChannel { get; }
    }
}
