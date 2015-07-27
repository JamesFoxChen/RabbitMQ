using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;

namespace _09_EasyNetQ_Message
{
    [Queue("EasyNetQueueName", ExchangeName = "EasyNetExchangeName")]
    public class TestMessageQueueName
    {
        public string Text { get; set; }
    }
}
