using System;
using Networking.Implementation;
namespace NetworkImplementationTestClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("CLIENT PROGRAM\nGive me your name: ");
            string Name = Console.ReadLine();
            Network.Init(Name);
            Client.Init(null);
            Client.StartClientOnNewThread("127.0.0.1", 9000);
            Console.WriteLine("CLIENT STARTED PRESS ESC TO QUIT");
            Console.Write("KEY: ");
            ConsoleKey Key = Console.ReadKey().Key;
            while (Key!=ConsoleKey.Escape)
            {
                if (Key == ConsoleKey.L)
                {
                    Console.WriteLine("\n\nPlayers:");
                    foreach (var pl in Network.Data.Players)
                    {
                        Console.WriteLine(pl.Name);
                    }
                }
                Console.WriteLine("\nCLIENT STARTED PRESS ESC TO QUIT");
                Console.Write("KEY: ");
                Key = Console.ReadKey().Key;
            }
            Client.StopClient();
        }
    }
}
