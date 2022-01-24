using Newtonsoft.Json;
using System;
using System.Xml.Serialization;

namespace Networking{
    [XmlRoot("Message")]
    [Serializable]
    public class KeepAliveRequestMessage : Message
    {
        public KeepAliveRequestMessage(int id): base(id,MessageType.Request,EDeliveryMethod.Reliable)
        {
        }

        public override string ToString()
        {
            return "KeepAliveRequestMessage";
        }
    }
}
