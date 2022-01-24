using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking
{
    public abstract class ClientNetworkStateManager: NetworkStateManager
    {
        protected Client client { get; private set; }

        public ClientNetworkStateManager(Client client, NetworkState State) : base(State)
        {
            this.client = client;
        }
    }
}
