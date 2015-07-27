
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.Topology;
using _09_EasyNetQ_Message;

namespace _09_EasyNetQ_Server
{
    class PoorSubscriber
    {
        public static void Receive()
        {
            using (IBus bus = RabbitHutch.CreateBus("host=localhost"))
            {
                IQueue yourQueue = bus.Advanced.QueueDeclare("MessageLogging");
                IExchange yourExchange = bus.Advanced.ExchangeDeclare("MessageLoggingEx", ExchangeType.Direct);
                bus.Advanced.Bind(yourExchange, yourQueue, "");
                IDisposable dispose = bus.Receive<RequestLog>("MessageLogging", msg =>
                                     {
                                         Console.ForegroundColor = ConsoleColor.Red;
                                         Console.WriteLine("Input: {0}", msg.Input);
                                         Console.WriteLine("Output: {0}", msg.Output);
                                         Console.ResetColor();
                                     });

                
            }
        }

        static void HandleMessage(RequestLog msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Input: {0}", msg.Input);
            Console.WriteLine("Output: {0}", msg.Output);
            Console.ResetColor();
        }
    }
}
