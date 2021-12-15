using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SocketServer server = new SocketServer();
            server.Start();
            Console.WriteLine("Echo Server is running.");
            Console.ReadLine();
        }
    }
}
