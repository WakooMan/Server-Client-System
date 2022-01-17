using Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class TestServerStateManager : NetworkStateManager
    {
        public TestServerStateManager(SocketServer Server) : base(Server)
        {
        }
        public TestServerStateManager(NetworkState state, SocketServer Server) : base(state, Server)
        {
        }

        public override void PopState()
        {
            throw new NotImplementedException();
        }

        public override void PushNextState()
        {
            throw new NotImplementedException();
        }
    }
}
