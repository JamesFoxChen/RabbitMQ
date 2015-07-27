using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.Loggers;
using _09_EasyNetQ_Message;

namespace _09_EasyNetQ_Server
{
    class _09_Server_Program
    {
        static void Main(string[] args)
        {
            //Subscriber.Receive();
            //NamedQueueSubscriber.Receive();
            //GlobalRequestLogSubscriber.Receive();
            PoorSubscriber.Receive();
        }
    }
}
