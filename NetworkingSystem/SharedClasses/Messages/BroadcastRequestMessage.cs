using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SharedClasses{
    [XmlRoot("Message")]
    public class BroadcastRequestMessage : Message
    {
        public BroadcastRequestMessage()
        {
            Type = MessageType.Request;
            Action = "Broadcast";
        }
    }
}
