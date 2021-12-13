﻿using System.Xml.Serialization;

namespace SharedClasses.Messages
{
    [XmlRoot("Message")]
    public class HeartBeatResponseMessage : Message
    {
        [XmlElement("Result")]
        public Result Result { get; set; }

        [XmlElement("POS")]
        public POSData POSData { get; set; }

        public HeartBeatResponseMessage()
        {
            Type=MessageType.Response;
            Action = "HeartBeat";
        }

    }
}
