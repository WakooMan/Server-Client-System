using Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class TestClientStateManager : NetworkStateManager
    {
        public TestClientStateManager(SocketClient Client) : base(Client)
        {
        }

        public TestClientStateManager(NetworkState state, SocketClient Client) : base(state, Client)
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
