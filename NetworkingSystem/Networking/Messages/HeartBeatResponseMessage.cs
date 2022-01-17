using Newtonsoft.Json;
using System;
using System.Xml.Serialization;

namespace Networking{
    [XmlRoot("Message")]
    [Serializable]
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

        public override string ToString()
        {
            return "HeartBeatResponseMessage";
        }
    }
}
