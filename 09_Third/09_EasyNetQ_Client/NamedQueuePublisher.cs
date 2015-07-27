using EasyNetQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _09_EasyNetQ_Message;

namespace _09_EasyNetQ_Client
{
    class NamedQueuePublisher
    {
        public static void Send()
        {
            using (var bus = RabbitHutch.CreateBus("host=localhost"))
            {
                var input = "";
                Console.WriteLine("Enter a message. 'quit' to quit.");
                while ((input = Console.ReadLine()) != "quit")
                {
                    bus.Publish(new TestMessageQueueName
                    {
                        Text = input
                    });
                }
            }
        }
    }
}
