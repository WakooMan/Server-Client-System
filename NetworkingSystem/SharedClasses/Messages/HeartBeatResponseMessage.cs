using Newtonsoft.Json;
using System.Xml.Serialization;

namespace Networking{
    [XmlRoot("Message")]
    public class HeartBeatResponseMessage : Message
    {
        [XmlElement("Result")]
        [JsonProperty("result")]
        public Result Result { get; set; }

        public HeartBeatResponseMessage()
        {
            Type=MessageType.Response;
            Action = "HeartBeat";
        }

    }
}
