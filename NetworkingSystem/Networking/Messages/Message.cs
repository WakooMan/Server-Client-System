using System;
using System.Xml.Serialization;

namespace Networking{
    public enum MessageType
    {
        Response,Request,Broadcast
    }

    public enum Status
    {
        Success,Failure
    }
    [Serializable]
    [XmlRoot("Message")]
    public abstract class Message
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("type")]
        public MessageType Type { get; set; }

        [XmlAttribute("action")]
        public string Action { get; set; }

        [XmlElement("POSData")]
        public POSData POSData { get; set; }

        public abstract override string ToString();
    }
    [Serializable]
    public class POSData
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
    }
    [Serializable]
    public class Result
    {
        [XmlAttribute("status")]
        public Status Status { get; set; }
    }
}
