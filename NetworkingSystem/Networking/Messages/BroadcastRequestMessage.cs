using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Networking{
    [XmlRoot("Message")]
    public class BroadcastRequestMessage : Message
    {
        [XmlElement("BroadcastMessage")]
        [JsonProperty("BroadcastMessage")]
        public BroadcastMessage BroadcastMessage { get; set; }

        public BroadcastRequestMessage()
        {
            Type = MessageType.Request;
            Action = "Broadcast";
            BroadcastMessage = null;
        }
        public BroadcastRequestMessage(BroadcastMessage broadcastMessage)
        {
            Type = MessageType.Request;
            Action = "Broadcast";
            BroadcastMessage = broadcastMessage;
        }
    }
}
