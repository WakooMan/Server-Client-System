using Networking;
using System;
using System.Threading.Tasks;

namespace Client
{
    public class MessageHandler : ClientMessageHandler
    {
        [ObjectMessageRoute("HeartBeatResponseMessage")]
        public static Task HandleMessage(HeartBeatResponseMessage response)
        {
            Console.WriteLine($"Received {response.Action}: {response?.Result?.Status}, {response?.Id}");
            return Task.CompletedTask;
        }
    }
}