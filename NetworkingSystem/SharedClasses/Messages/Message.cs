using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SharedClasses{
    public enum MessageType
    {
        Response,Request
    }

    public enum Status
    {
        Success,Failure
    }
    [XmlRoot("Message")]
    public abstract class Message
    {
        [XmlAttribute("id")]
        [JsonProperty("id")]
        public string Id { get; set; }

        [XmlAttribute("type")]
        [JsonProperty("type")]
        public MessageType Type { get; set; }

        [XmlAttribute("action")]
        [JsonProperty("action")]
        public string Action { get; set; }

        [XmlElement("POSData")]
        [JsonProperty("posData")]
        public POSData POSData { get; set; }
    }

    public class POSData
    {
        [XmlAttribute("id")]
        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class Result
    {
        [XmlAttribute("status")]
        [JsonProperty("status")]
        public Status Status { get; set; }
    }
}
