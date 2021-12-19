using Newtonsoft.Json;
using System.Xml.Serialization;

namespace Networking{
    [XmlRoot("Message")]
    public class BroadcastResponseMessage : Message
    {
        [XmlElement("Result")]
        [JsonProperty("result")]
        public Result Result { get; set; }

        public BroadcastResponseMessage()
        {
            Type = MessageType.Response;
            Action = "Broadcast";
        }

    }
}
