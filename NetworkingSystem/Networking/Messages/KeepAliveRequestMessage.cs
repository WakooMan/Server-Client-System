using Newtonsoft.Json;
using System;
using System.Xml.Serialization;

namespace Networking{
    [XmlRoot("Message")]
    [Serializable]
    public class KeepAliveRequestMessage : ManuallySerializedMessage
    {
        public KeepAliveRequestMessage() : base(0,MessageType.Request, EDeliveryMethod.Reliable) { }

        public override void Deserialize(byte[] bytes)
        {
            base.Deserialize(bytes);
        }

        public override string ToString()
        {
            return "KeepAliveRequestMessage";
        }
    }
}
