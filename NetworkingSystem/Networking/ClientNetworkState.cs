using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking
{
    public abstract class ClientNetworkState: NetworkState
    {
        protected Client client { get; private set; }
        protected ClientNetworkState(Client client)
        {
            this.client = client;
        }
    }
}
