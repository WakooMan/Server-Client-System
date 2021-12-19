using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Networking{
    public class ClientChannel<TProtocol,TMessageType>: NetworkChannel<TProtocol,TMessageType>
        where TProtocol : Protocol<TMessageType>, new()
    {
        public async Task ConnectAsync(IPEndPoint EndPoint)
        { 
            var socket = new Socket(EndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            await socket.ConnectAsync(EndPoint).ConfigureAwait(false);
            Attach(socket);
        }
    }
}
