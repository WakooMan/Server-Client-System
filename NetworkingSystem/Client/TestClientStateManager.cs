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
        public TestClientStateManager(Networking.Client Client) : base(Client,null)
        {
        }

        public TestClientStateManager(Networking.Client Client, NetworkState state) : base(Client,state)
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
