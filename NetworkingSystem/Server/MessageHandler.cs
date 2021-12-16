﻿using SharedClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public  class MessageHandler
    {
        [XPathRoute("/Message[@type='Request' and @action='HeartBeat']")]
        [JsonRoute("$.action","HeartBeat")]
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

        [XPathRoute("/Message[@type='Request' and @action='SubmitBasket']")]
        [JsonRoute("$.action", "SubmitBasket")]
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

        private static void Received<T>(T m) where T : Message => Console.WriteLine($"Received {typeof(T).Name}: POS ID [ {m.POSData.Id} ], Action [ {m.Action} ], ID [ {m.Id} ] ");
        private static void Sending<T>(T m) where T : Message => Console.WriteLine($"Sending {typeof(T).Name}: POS ID [ {m.POSData.Id} ], Action [ {m.Action} ], ID [ {m.Id} ] ");
    }
}
