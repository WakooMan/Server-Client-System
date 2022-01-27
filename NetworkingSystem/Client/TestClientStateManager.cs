using Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class TestClientStateManager : ClientNetworkStateManager
    {
        public TestClientStateManager(IClient Client) : base(Client,null)
        {
        }

        public TestClientStateManager(IClient Client, NetworkState state) : base(Client,state)
        {
        }

        public override void PopState()
        {
            
        }

        public override void PushNextState()
        {
            
        }
    }
}
