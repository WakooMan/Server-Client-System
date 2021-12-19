using Newtonsoft.Json;
using System.Xml.Serialization;

namespace SharedClasses{
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
