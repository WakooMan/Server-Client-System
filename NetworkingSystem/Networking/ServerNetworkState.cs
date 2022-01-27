using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking
{
    public abstract class ServerNetworkState: NetworkState
    {
        protected IServer server { get; private set; }
        protected ServerNetworkState(IServer server)
        {
            this.server = server;
        }
    }
}
