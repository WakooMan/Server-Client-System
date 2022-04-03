using System;
using ProtoBuf;

namespace Networking.Implementation.Messages.FromServerMessages
{
    [ProtoContract]
    public class PlayerDisconnectedMessage : Message
    {
        [ProtoMember(1)]
        public Guid ID;
        public PlayerDisconnectedMessage() : base(MessageType.Broadcast, EDeliveryMethod.Reliable)
        {
        }

        public PlayerDisconnectedMessage(Guid id) : base(MessageType.Broadcast, EDeliveryMethod.Reliable)
        {
            ID = id;
        }
    }
}
