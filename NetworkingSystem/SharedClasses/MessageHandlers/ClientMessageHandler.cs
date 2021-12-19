using System;
using System.Threading.Tasks;

namespace Networking
{
    public class ClientMessageHandler
    {
        //Handler on the 'Client' side of the system
        [XPathRoute("/Message[@type='Response' and @action='HeartBeat']")]
        public static Task HandleMessage(HeartBeatResponseMessage response)
        {
            Console.WriteLine($"Received {response.Action}: {response?.Result?.Status}, {response?.Id}");
            return Task.CompletedTask;
        }

        [XPathRoute("/Message[@type='Response' and @action='Broadcast']")]
        public static Task HandleMessage(BroadcastResponseMessage response)
        {
            Console.WriteLine($"Received {response.Action}: {response?.Result?.Status}, {response?.Id}");
            return Task.CompletedTask;
        }

        [XPathRoute("/Message[@type='Broadcast' and @action='Broadcast']")]
        public static Task HandleMessage(BroadcastMessage response)
        {
            Console.WriteLine($"Received {response.Action} Message: {response?.Id}");
            return Task.CompletedTask;
        }
    }
}
