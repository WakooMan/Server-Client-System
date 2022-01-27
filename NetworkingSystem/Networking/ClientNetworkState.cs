using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking
{
    public abstract class ClientNetworkState: NetworkState
    {
        protected IClient client { get; private set; }
        protected ClientNetworkState(IClient client)
        {
            this.client = client;
        }
    }
}
