using ProtoBuf;

namespace Networking.Implementation.Messages.FromServerMessages
{
    [ProtoContract]
    public class PlayerJoinedMessage: Message
    {
        [ProtoMember(1)]
        public PlayerSerializer Player;
        public PlayerJoinedMessage() : base(MessageType.Broadcast, EDeliveryMethod.Reliable)
        {
        }

        public PlayerJoinedMessage(Player player) : base(MessageType.Broadcast,EDeliveryMethod.Reliable)
        {
            Player = new PlayerSerializer(player);
        }
    }
}