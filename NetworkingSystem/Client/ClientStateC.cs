using Networking;
using Networking.Messages;
using System.Threading;

namespace Client
{
    internal class ClientStateC : ClientNetworkState
    {
        public ClientStateC(IClient client) :base(client){ }

        private int bytenum = 100;
        public override void ExecuteStuff()
        {
            client.Send(new RandomMessage(new byte[bytenum]));
            Thread.Sleep(5000);
        }

        public override string ToString() => "ClientStateC";
    }
}
