using Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            TestServer Server = new TestServer(4, () => { }, () => { });
            Server.Bind<ServerMessageHandler<ServerStates>>();
            var serverTask =Server.StartAsync(9000,cancellationTokenSource.Token);
            Server.ServerLoop(serverTask,cancellationTokenSource);
            
        }
    }
}
