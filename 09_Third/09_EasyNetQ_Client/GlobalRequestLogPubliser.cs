using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;
using Newtonsoft.Json;
using _09_EasyNetQ_Message;

namespace _09_EasyNetQ_Client
{
    public class RequestLogPubliser
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
                //var input = "";
                //Console.WriteLine("Enter a message. 'quit' to quit.");
                //while ((input = Console.ReadLine()) != "quit")
                //{

                //EasyNetQ.JsonSerializer s = new EasyNetQ.JsonSerializer(new TypeNameSerializer());

                bus.Publish<RequestLog>(request);
            }
        }
    }
}
