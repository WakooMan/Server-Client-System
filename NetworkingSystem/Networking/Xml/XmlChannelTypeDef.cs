using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Networking{
    public class XmlChannel : TCPNetworkChannel<XmlMessageProtocol, XDocument>
    {
        public XmlChannel(Guid id, Action receivedMessage) : base(id, receivedMessage)
        {
        }
    }
    public class XmlClientChannel : ClientTCPChannel<XmlMessageProtocol, XDocument>
    {
        public XmlClientChannel(Guid id, Action ReceivedMessage) : base(id, ReceivedMessage)
        {
        }
    }
}
