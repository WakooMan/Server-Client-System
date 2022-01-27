using Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class TestServerStateManager : ServerNetworkStateManager
    {
        public TestServerStateManager(IServer Server): base(Server,null)
        {
        }
        public TestServerStateManager(IServer Server, NetworkState state) : base(Server, state)
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
