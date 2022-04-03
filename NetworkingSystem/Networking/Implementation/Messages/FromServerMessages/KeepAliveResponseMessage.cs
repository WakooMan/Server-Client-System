using ProtoBuf;

namespace Networking.Implementation.Messages.FromServerMessages
{

    [ProtoContract]
    public class KeepAliveResponseMessage : Message
    {
        [ProtoMember(1)]
        public Result Result { get; set; }

        public KeepAliveResponseMessage() : base(MessageType.Response, EDeliveryMethod.Reliable) { }

        public KeepAliveResponseMessage(Result result) : base(MessageType.Response,EDeliveryMethod.Reliable)
        {
            Result = result;
        }

        public override string ToString()
        {
            return "KeepAliveResponseMessage";
        }
    }
}
