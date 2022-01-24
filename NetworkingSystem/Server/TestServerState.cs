using Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class TestServerState : ServerNetworkState
    {
        public TestServerState(Networking.Server server) : base(server)
        {
        }

        public override void ExecuteStuff()
        {
           
        }

        public override string ToString()
        {
            return "TestServerState";
        }
    }
}
