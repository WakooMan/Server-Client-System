using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SharedClasses.Messages
{
    [XmlRoot("Message")]
    public class SubmitBasketRequestMessage : Message
    {
        [XmlElement("POS")]
        public POSData POSData { get; set; }

        public SubmitBasketRequestMessage()
        {
            Type = MessageType.Request;
            Action = "SubmitBasket";
        }
    }
}
