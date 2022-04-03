using System;

namespace Networking.Implementation
{
    public class Player
    {
        private Guid id;
        public string Name { get; private set; }
        public Guid ID 
        { 
            get 
            { 
                return id; 
            } 
            set 
            {
                if (id == Guid.Empty)
                    id = value;
                else
                    throw new ClientAlreadyHasGuidException();
            } 
        }

        public Player(string name)
        {
            id=Guid.Empty;
            Name = name;
        }

        public Player(Guid id, string name)
        {
            this.id = id;
            Name = name;
        }
    }
}