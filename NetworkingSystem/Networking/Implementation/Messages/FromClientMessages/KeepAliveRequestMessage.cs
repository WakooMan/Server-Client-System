using ProtoBuf;
using ProtoBuf.Meta;

namespace Networking.Implementation.Messages.FromClientMessages
{
    [ProtoContract]
    public class KeepAliveRequestMessage : Message
    {
        public KeepAliveRequestMessage() : base(MessageType.Request,EDeliveryMethod.Reliable) { }

        public override string ToString()
        {
            return "KeepAliveRequestMessage";
        }
    }
}
