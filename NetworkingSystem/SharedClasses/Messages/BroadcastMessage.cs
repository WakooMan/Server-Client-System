using Newtonsoft.Json;
using System;
using System.Xml.Serialization;

namespace Networking
{
    //This class is an abstract class for a broadcast message
    //The TEnum type is an enumeration that represents the type of the message.
    //This TEnum are the message types of the current state of the network.
    //For example in lobby we use different messages, than in battles, the TEnum is what differs it from each other.
    [Serializable]
    [XmlRoot("Message")]
    public class BroadcastMessage : Message
    {
        [XmlElement("object")]
        [JsonProperty("object")]
        public object Object { get; set; }
        public BroadcastMessage() 
        {
            Object = null;
            Type = MessageType.Broadcast;
            Action = "Broadcast";
        }

        public BroadcastMessage(object obj)
        {
            Object = obj;
            Type = MessageType.Broadcast;
            Action = "Broadcast";
        }
    }
}
