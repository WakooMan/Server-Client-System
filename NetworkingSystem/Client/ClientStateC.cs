using Networking;
using System.Threading;

namespace Client
{
    internal class ClientStateC : ClientNetworkState
    {
        public ClientStateC(IClient client) :base(client){ }

        public override void ExecuteStuff()
        {
            
        }

        public override string ToString() => "ClientStateC";
    }
}
