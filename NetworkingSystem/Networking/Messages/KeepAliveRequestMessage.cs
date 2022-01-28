using Newtonsoft.Json;
using System;
using System.Xml.Serialization;

namespace Networking{
    [XmlRoot("Message")]
    [Serializable]
    public class KeepAliveRequestMessage : ReliableMessage
    {
        public KeepAliveRequestMessage() : base(0,MessageType.Request) { }

        public override string ToString()
        {
            return "KeepAliveRequestMessage";
        }
    }
}
