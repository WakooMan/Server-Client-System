using Newtonsoft.Json;
using ProtoBuf;
using System;
using System.Xml.Serialization;

namespace Networking{
    [ProtoContract]
    public class KeepAliveRequestMessage : Message
    {
        public KeepAliveRequestMessage() : base(0,MessageType.Request,EDeliveryMethod.Reliable) { }

        public override string ToString()
        {
            return "KeepAliveRequestMessage";
        }
    }
}
