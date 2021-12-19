using Newtonsoft.Json;
using System.Xml.Serialization;

namespace Networking{
    [XmlRoot("Message")]
    public class HeartBeatRequestMessage : Message
    {
        public HeartBeatRequestMessage()
        {
            Type=MessageType.Request;
            Action = "HeartBeat";
        }
    }
}
