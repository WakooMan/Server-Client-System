using System;
using System.Collections.Generic;
using System.Linq;

namespace Networking.Implementation
{
    public class NetworkData
    {
        public Player MainPlayer { get; private set; }
        private Dictionary<Guid,Player> players;

        public NetworkData(string Name)
        {
            players = new Dictionary<Guid,Player>();
            MainPlayer = new Player(Name); //TODO: Get Steam name and stuff.
        }

        public void Add(Player player)
        {
            players.Add(player.ID,player);
        }

        public void Remove(Guid ID)
        {
            players.Remove(ID);
        }

        public IEnumerable<Player> Players
        {
            get { return players.Values.AsEnumerable(); }
        }

        public Player Search(string Name) 
        {
            foreach (var pl in Players)
            {
                if(pl.Name==Name)
                    return pl;
            }
            return null;
        }
        public Player Search(Guid ID)
        {
            if (players.ContainsKey(ID))
                return players[ID];
            else
                return null;
        }
    }
}
