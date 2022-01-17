using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Networking{
    public class ClientTCPChannel<TProtocol,TMessageType>: TCPNetworkChannel<TProtocol,TMessageType>
        where TProtocol : Protocol<TMessageType>, new()
    {
        public ClientTCPChannel(Guid id,Action ReceivedMessage) : base(id, ReceivedMessage)
        {
        }

        public async Task ConnectAsync(IPEndPoint EndPoint)
        { 
            var socket = new Socket(EndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            await socket.ConnectAsync(EndPoint).ConfigureAwait(false);
            Attach(socket);
        }
    }
}
