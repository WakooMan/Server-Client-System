using System.Xml.Serialization;

namespace SharedClasses.Messages
{
    [XmlRoot("Message")]
    public class SubmitBasketResponseMessage : Message
    {
        [XmlElement("Result")]
        public Result Result { get; set; }

        [XmlElement("POS")]
        public POSData POSData { get; set; }

        public SubmitBasketResponseMessage()
        {
            Type = MessageType.Response;
            Action = "SubmitBasket";
        }

    }
}
