using EasyNetQ;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace _09_EasyNetQ_Message
{
    [Queue("MessageLogging", ExchangeName = "MessageLoggingEx")]
    public class RequestLog
    {
        public string Id { get; set; }

        public string strId { get; set; }

        public DateTime RequestDate { get; set; }

        public long ProcessTime { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public string CMac { get; set; }

        public string RMac { get; set; }

        public string Input { get; set; }

        public int ReturnCode { get; set; }

        public string Output { get; set; }

        public string RequestUrl { get; set; }

        public string Version { get; set; }

        public string Ip { get; set; }

        public string RequestId { get; set; }
    }
}
