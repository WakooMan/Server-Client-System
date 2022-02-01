using ProtoBuf;
using System;

namespace Networking{

    [ProtoContract]
    public class KeepAliveResponseMessage : Message
    {
        [ProtoMember(1)]
        public Result Result { get; set; }

        public KeepAliveResponseMessage() : base(1, MessageType.Response, EDeliveryMethod.Reliable) { }

        public KeepAliveResponseMessage(Result result) : base(1,MessageType.Response,EDeliveryMethod.Reliable)
        {
            Result = result;
        }

        public override string ToString()
        {
            return "KeepAliveResponseMessage";
        }
    }
}
