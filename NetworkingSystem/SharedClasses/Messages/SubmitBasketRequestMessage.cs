﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SharedClasses{
    [XmlRoot("Message")]
    public class SubmitBasketRequestMessage : Message
    {
        public SubmitBasketRequestMessage()
        {
            Type = MessageType.Request;
            Action = "SubmitBasket";
        }
    }
}
