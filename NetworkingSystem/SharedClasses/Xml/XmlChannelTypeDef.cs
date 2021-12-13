using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SharedClasses.Xml
{
    public class XmlChannel : NetworkChannel<XmlMessageProtocol, XDocument> { }
    public class XmlClientChannel : ClientChannel<XmlMessageProtocol, XDocument> { }
}
