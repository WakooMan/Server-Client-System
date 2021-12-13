using SharedClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public static class MessageHandler
    {
        //Handler on the 'Client' side of the system
        [XPathRoute("/Message[@type='Response' and @action='HeartBeat']")]
        public static Task HandleMessage(HeartBeatResponseMessage response)
        {
            Console.WriteLine($"Received {response.Action}: {response?.Result?.Status}, {response?.Id}");
            return Task.CompletedTask;
        }

        [XPathRoute("/Message[@type='Response' and @action='SubmitBasket']")]
        public static Task HandleMessage(SubmitBasketResponseMessage response)
        {
            Console.WriteLine($"Received {response.Action}: {response?.Result?.Status}, {response?.Id}");
            return Task.CompletedTask;
        }
    }
}
