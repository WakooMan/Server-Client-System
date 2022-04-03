using System;
using Networking.Implementation;

namespace NetworkImplementationTestServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("SERVER PROGRAM\nGive me your name: ");
            string Name = Console.ReadLine();
            Network.Init(Name);
            Server.Init();
            Server.StartServerOnNewThread(4,9000);
            Console.WriteLine("SERVER STARTED PRESS ESC TO QUIT");
            Console.Write("KEY: ");
            ConsoleKey Key = Console.ReadKey().Key;
            while (Key != ConsoleKey.Escape)
            {
                if (Key == ConsoleKey.L)
                {
                    Console.WriteLine("\n\nPlayers:");
                    foreach (var pl in Network.Data.Players)
                    {
                        Console.WriteLine(pl.Name);
                    }
                }
                Console.WriteLine("\nSERVER STARTED PRESS ESC TO QUIT");
                Console.Write("KEY: ");
                Key = Console.ReadKey().Key;
            }
            Server.StopServer();
        }
    }
}
