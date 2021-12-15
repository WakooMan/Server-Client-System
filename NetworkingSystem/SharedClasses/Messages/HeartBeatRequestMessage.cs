using Newtonsoft.Json;
using System.Xml.Serialization;

namespace SharedClasses{
    [XmlRoot("Message")]
    public class HeartBeatRequestMessage : Message
    {
        [XmlElement("POSData")]
        [JsonProperty("posData")]
        public POSData POSData { get; set; }

        public HeartBeatRequestMessage()
        {
            Type=MessageType.Request;
            Action = "HeartBeat";
        }
    }
}
