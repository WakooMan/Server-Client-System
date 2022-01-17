using Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class TestServer : SocketServer
    {
        public TestServer(int maxClients, Action _onClientConnected, Action _onClientDisconnected) : base(maxClients) { ServerStateManager = new TestServerStateManager(new TestServerState(this), this); Bind<MessageHandler>(); }
        public async override Task ServerLoop(Task serverTask,CancellationTokenSource cancellationTokenSource)
        {
            do
            {
                await ServerStateManager.CurrState.ExecuteStuff();
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
