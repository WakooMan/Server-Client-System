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
        public int Id { get; set; }

        [XmlAttribute("type")]
        public MessageType Type { get; set; }

        [XmlAttribute("emethod")]
        public EDeliveryMethod EMethod { get; set; }

        public Message(int id, MessageType type, EDeliveryMethod eMethod) 
        {
            Id = id;
            Type = type;
            EMethod = eMethod;
        }
        public abstract override string ToString();
    }
    [Serializable]
    public class Result
    {
        [XmlAttribute("status")]
        public Status Status { get; set; }

        public Result(Status status) { Status = status; }
    }
}
