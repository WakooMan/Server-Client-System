using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SharedClasses
{
    [XmlRoot("Message")]
    public class BroadcastMessage : Message
    {
        public BroadcastMessage() 
        {
            Type = MessageType.Broadcast;
            Action = "Broadcast";
        }
        public BroadcastMessage(BroadcastRequestMessage requestmessage)
        {
            Id = requestmessage.Id;
            POSData = requestmessage.POSData;
            Type = MessageType.Broadcast;
            Action = requestmessage.Action;
        }
    }
}
