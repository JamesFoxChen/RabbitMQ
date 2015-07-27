using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.Topology;
using _09_EasyNetQ_Message;

namespace _09_EasyNetQ_Client
{
    class PoorPublisher
    {
        public static void Send()
        {
            var request = new RequestLog
            {
                Input = "input info",
                Output = "output info",
                RequestDate = DateTime.Now
            };

            using (var bus = RabbitHutch.CreateBus("host=localhost"))
            {
                IQueue yourQueue = bus.Advanced.QueueDeclare("MessageLogging");
                IExchange yourExchange = bus.Advanced.ExchangeDeclare("MessageLoggingEx", ExchangeType.Direct);
                bus.Advanced.Bind(yourExchange, yourQueue, "");
                bus.Send<RequestLog>("MessageLogging", request);
            }
        }
    }
}
