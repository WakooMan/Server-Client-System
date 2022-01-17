using Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class TestServerState : NetworkState
    {
        public TestServerState(SocketServer server) : base(server)
        {
        }

        public override Task ExecuteStuff()
        {
            return Task.CompletedTask;
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}
