using Newtonsoft.Json;
using System;
using System.Xml.Serialization;

namespace Networking{
    [XmlRoot("Message")]
    [Serializable]
    public class KeepAliveResponseMessage : Message
    {
        [XmlElement("Result")]
        [JsonProperty("result")]
        public Result Result { get; set; }

        public KeepAliveResponseMessage(int id,Result result) : base(id,MessageType.Response,EDeliveryMethod.Reliable)
        {
            Type=MessageType.Response;
            Result = result;
        }

        public override string ToString()
        {
            return "KeepAliveResponseMessage";
        }
    }
}
