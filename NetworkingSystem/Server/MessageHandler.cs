using SharedClasses.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public static  class MessageHandler
    {
        [Route("/Message[@type='Request' and @action='HeartBeat']")]
        public static Task<HeartBeatResponseMessage> HandleMessage(HeartBeatRequestMessage request)
        {
            Received(request);
            var response = new HeartBeatResponseMessage
            {
                Id= request.Id,
                POSData= request.POSData,
                Result = new Result { Status = Status.Success}
            };
            Sending(response);
            return Task.FromResult(response);
        }

        [Route("/Message[@type='Request' and @action='SubmitBasket']")]
        public static Task<SubmitBasketResponseMessage> HandleMessage(SubmitBasketRequestMessage request)
        {
            Received(request);
            var response = new SubmitBasketResponseMessage
            {
                Id = request.Id,
                POSData = request.POSData,
                Result = new Result { Status = Status.Success }
            };
            Sending(response);
            return Task.FromResult(response);
        }

        private static void Received<T>(T m) where T : Message => Console.WriteLine($"Received {typeof(T).Name} => Action[ {m.Action} ], RequestId[ {m.Id} ] ");
        private static void Sending<T>(T m) where T : Message => Console.WriteLine($"Sending {typeof(T).Name} => Action[ {m.Action} ], RequestId[ {m.Id} ] ");
    }
}
