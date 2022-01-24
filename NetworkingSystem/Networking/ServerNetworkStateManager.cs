using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking
{
    public abstract class ServerNetworkStateManager: NetworkStateManager
    {
        protected Server server { get; private set; }

        public ServerNetworkStateManager(Server server, NetworkState State) : base(State)
        {
            this.server = server;
        }
    }
}
