using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking
{
    public abstract class NetworkState
    {
        public SocketClient Client { get; private set; }
        public SocketServer Server { get; private set; }

        protected NetworkState(SocketServer server)
        {
            Server = server;
            Client = null;
        }

        protected NetworkState(SocketClient client)
        {
            Client = client;
            Server = null;
        }

        public bool IsThisState(string StateName)
        {
            return StateName == ToString();
        }
#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
        public abstract string ToString();
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword
        public abstract Task ExecuteStuff();
    }
}
