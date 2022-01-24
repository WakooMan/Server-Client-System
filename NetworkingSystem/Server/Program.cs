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
        static void Main(string[] args)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            TestServer Server = new TestServer(4, () => { }, () => { });
            Server.Start(9000);
            Server.StartServerLoopOnNewThread();
            while (Server.Running)
            {
                Server.Update();
                Thread.Sleep(100);
            }
        }
    }
}
