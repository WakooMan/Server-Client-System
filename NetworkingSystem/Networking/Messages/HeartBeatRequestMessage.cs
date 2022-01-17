using Newtonsoft.Json;
using System;
using System.Xml.Serialization;

namespace Networking{
    [XmlRoot("Message")]
    [Serializable]
    public class HeartBeatRequestMessage : Message
    {
        public HeartBeatRequestMessage()
        {
            Type=MessageType.Request;
            Action = "HeartBeat";
        }

        public override string ToString()
        {
            return "HeartBeatRequestMessage";
        }
    }
}
