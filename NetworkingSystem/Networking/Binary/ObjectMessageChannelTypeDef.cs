using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Networking
{
    public class ObjectMessageTCPChannel : TCPNetworkChannel<ObjectMessageProtocol, object>
    {
        public ObjectMessageTCPChannel(Guid id, Action receivedMessage) : base(id, receivedMessage)
        {
        }
    }

    public class ObjectMessageClientTCPChannel : ClientTCPChannel<ObjectMessageProtocol, object>
    {
        public ObjectMessageClientTCPChannel(Guid id, Action ReceivedMessage) : base(id, ReceivedMessage)
        {
        }
    }

    public class ObjectMessageUDPChannel : UDPNetworkChannel<ObjectMessageProtocol, object>
    {
        public ObjectMessageUDPChannel(Guid id, Action receivedMessage) : base(id, receivedMessage)
        {
        }
    }
}
