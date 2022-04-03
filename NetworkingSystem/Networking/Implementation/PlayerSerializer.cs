using ProtoBuf;
using System;

namespace Networking.Implementation
{
    [ProtoContract]
    public class PlayerSerializer
    {
        [ProtoMember(1)]
        public string Name;
        [ProtoMember(2)]
        public Guid ID;

        public PlayerSerializer(Player pl)
        {
            Name = pl.Name;
            ID = pl.ID;
        }

        public PlayerSerializer()
        {
        }

        public Player ReturnAsPlayer() 
        {
            return new Player(ID, Name);
        }
    }
}
