using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;
using _09_EasyNetQ_Message;

namespace _09_EasyNetQ_Client
{
    class Publisher
    {
        public static void Send()
        {
            using (var bus = RabbitHutch.CreateBus("host=localhost"))
            {
                var input = "";
                Console.WriteLine("Enter a message. 'quit' to quit.");
                while ((input = Console.ReadLine()) != "quit")
                {
                    bus.Publish(new Message
                    {
                        Text = input
                    });
                }
            }
        }
    }
}
