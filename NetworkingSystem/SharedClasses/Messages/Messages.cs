using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SharedClasses.Messages
{
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
        public string Id { get; set; }

        [XmlAttribute("type")]
        public MessageType Type { get; set; }

        [XmlAttribute("action")]
        public string Action { get; set; }
    }

    public class POSData
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
    }

    public class Result
    {
        [XmlAttribute("status")]
        public Status Status { get; set; }
    }
}
