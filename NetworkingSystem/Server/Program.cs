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
        public static XmlSocketServer Server;
        //public static JsonSocketServer Server;
        static async Task Main(string[] args)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            Server = new XmlSocketServer(4, () => { }, () => { });
            //Server = new JsonSocketServer(4, () => { }, () => { });
            Server.Bind<MessageHandler>();
            var serverTask =Server.StartAsync(9000,cancellationTokenSource.Token);
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
