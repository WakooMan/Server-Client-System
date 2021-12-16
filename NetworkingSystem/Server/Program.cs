using SharedClasses;
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
            //XmlSocketServer server = new XmlSocketServer();
            JsonSocketServer server = new JsonSocketServer(2);
            server.Bind<MessageHandler>();
            var serverTask =server.StartAsync(9000,cancellationTokenSource.Token);
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
