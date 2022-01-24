using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking
{
    public abstract class ServerNetworkState: NetworkState
    {
        protected Server server { get; private set; }
        protected ServerNetworkState(Server server)
        {
            this.server = server;
        }
    }
}
