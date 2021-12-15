using Newtonsoft.Json;
using System.Xml.Serialization;

namespace SharedClasses{
    [XmlRoot("Message")]
    public class HeartBeatResponseMessage : Message
    {
        [XmlElement("Result")]
        [JsonProperty("result")]
        public Result Result { get; set; }

        [XmlElement("POSData")]
        [JsonProperty("posData")]
        public POSData POSData { get; set; }

        public HeartBeatResponseMessage()
        {
            Type=MessageType.Response;
            Action = "HeartBeat";
        }

    }
}
