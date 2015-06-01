using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11_MyMessage.Common.QueueModel
{
    public class LogQueueModel
    {

        public string RequestId { get; set; }

        public string RequestMsg { get; set; }

        public string ResponseMsg { get; set; }

        public DateTime Date { get; set; }
    }
}
