using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;
using _09_EasyNetQ_Message;

namespace _09_EasyNetQ_Client
{
    class _09_Client_Program
    {
        static void Main(string[] args)
        {
            //Publisher.Send();
            //NamedQueuePublisher.Send();
            //GlobalRequestLogPubliser.Send();
            PoorPublisher.Send();
        }
    }
}
