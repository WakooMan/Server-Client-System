using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Networking{
    public class XmlMessageProtocol : Protocol<XDocument>
    {

        protected override XDocument Decode(byte[] Message)
        {
            var XMLData = Encoding.UTF8.GetString(Message);
            var XMLReader = XmlReader.Create(new StringReader(XMLData), new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore });
            return XDocument.Load(XMLReader);
        }

        protected override byte[] EncodeBody<T>(T message)
        {
            if (message is XDocument)
                return Encoding.UTF8.GetBytes(message.ToString());
            else
                return Encoding.UTF8.GetBytes(XmlSerialization.Serialize(message).ToString());
        }
    }
}
