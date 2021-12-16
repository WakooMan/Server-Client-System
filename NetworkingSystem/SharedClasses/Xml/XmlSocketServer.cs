using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SharedClasses
{
    public class XmlSocketServer : SocketServer<XmlChannel, XmlMessageProtocol, XDocument, XDocumentMessageDispatcher>
    {
        public XmlSocketServer(int maxClients) : base(maxClients){ }
    }
}
