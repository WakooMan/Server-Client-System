using Networking;
using Networking.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class TestServer : Server<Message> 
    {
        public TestServer(int maxClients, Action _onClientConnected, Action _onClientDisconnected) : base(maxClients) { ServerStateManager = new TestServerStateManager(this, new TestServerState(this)); Bind<MessageHandler>(); }
        protected override void ServerLoop()
        {
            do
            {
                ServerStateManager.CurrState.ExecuteStuff();
                Console.WriteLine("Echo Server is running. Press X to Exit");
                var Key = Console.ReadKey();
                if (Key.Key == ConsoleKey.X)
                {
                    Console.WriteLine("Server Shutdown");
                    break;
                }
            } while (true);
        }
    }
}
