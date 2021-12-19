using Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class TestServer : SocketServer<ServerStates>
    {
        public TestServer(int maxClients, Action _onClientConnected, Action _onClientDisconnected) : base(maxClients,_onClientConnected,  _onClientDisconnected) { }
        public async override void ServerLoop(Task serverTask,CancellationTokenSource cancellationTokenSource)
        {
            do
            {
                Console.WriteLine("Echo Server is running. Press X to Exit");
                var Key = Console.ReadKey();
                if (Key.Key == ConsoleKey.X)
                {
                    cancellationTokenSource.Cancel();
                    await serverTask;
                    Console.WriteLine("Server Shutdown");
                    break;
                }
            } while (true);
        }
    }
}
