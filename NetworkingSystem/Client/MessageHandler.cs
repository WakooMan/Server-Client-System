using SharedClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class MessageHandler
    {
        //Handler on the 'Client' side of the system
        [XPathRoute("/Message[@type='Response' and @action='HeartBeat']")]
        [JsonRoute("$.action","HeartBeat")]
        public static Task HandleMessage(HeartBeatResponseMessage response)
        {
            Console.WriteLine($"Received {response.Action}: {response?.Result?.Status}, {response?.Id}");
            return Task.CompletedTask;
        }

        [XPathRoute("/Message[@type='Response' and @action='Broadcast']")]
        [JsonRoute("$.action", "Broadcast")]
        public static Task HandleMessage(BroadcastResponseMessage response)
        {
            Console.WriteLine($"Received {response.Action}: {response?.Result?.Status}, {response?.Id}");
            return Task.CompletedTask;
        }

        [XPathRoute("/Message[@type='Broadcast' and @action='Broadcast']")]
        [JsonRoute("$.action", "Broadcast")]
        public static Task HandleMessage(BroadcastMessage response)
        {
            Console.WriteLine($"Received {response.Action} Message: {response?.Id}");
            return Task.CompletedTask;
        }
    }
}
