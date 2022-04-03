using ProtoBuf;
using ProtoBuf.Meta;

namespace Networking.Implementation.Messages.FromClientMessages
{
    [ProtoContract]
    public class NetworkDataRequestMessage : Message
    {
        [ProtoMember(1)]
        public PlayerSerializer Player;

        public NetworkDataRequestMessage() : base(MessageType.Request, EDeliveryMethod.Reliable)
        {}
        public NetworkDataRequestMessage(Player player) : base(MessageType.Request, EDeliveryMethod.Reliable)
        {
            Player = new PlayerSerializer(player);
        }
    }
}
