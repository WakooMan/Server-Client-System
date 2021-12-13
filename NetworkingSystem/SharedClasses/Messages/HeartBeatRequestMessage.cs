using System.Xml.Serialization;

namespace SharedClasses.Messages
{
    [XmlRoot("Message")]
    public class HeartBeatRequestMessage : Message
    {
        [XmlElement("POS")]
        public POSData POSData { get; set; }

        public HeartBeatRequestMessage()
        {
            Type=MessageType.Request;
            Action = "HeartBeat";
        }
    }
}
