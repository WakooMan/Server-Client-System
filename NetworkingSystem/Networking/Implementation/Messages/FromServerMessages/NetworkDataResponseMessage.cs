using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Networking.Implementation.Messages.FromServerMessages
{
    [ProtoContract]
    public class NetworkDataResponseMessage : Message
    {
        [ProtoMember(1)]
        public List<PlayerSerializer> Players;
        [ProtoMember(2)]
        public Guid PlayerID;
        public NetworkDataResponseMessage() : base(MessageType.Response, EDeliveryMethod.Reliable)
        {
        }

        public NetworkDataResponseMessage(Guid id,List<Player> players) : base(MessageType.Response, EDeliveryMethod.Reliable)
        {
            PlayerID = id;
            Players = new List<PlayerSerializer>();
            players.ForEach(player => Players.Add(new PlayerSerializer(player)));
        }
    }
}
