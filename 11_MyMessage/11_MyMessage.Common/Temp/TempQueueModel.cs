using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _11_MyMessage.Common.Temp
{
    public class TempQueueModel
    {
        public TempQueueModel()
        {
            LogDate = DateTime.Now;
        }

        /// <summary>
        /// 类型
        /// </summary>
        public Enum_QueueType Type { get; set; }

        /// <summary>
        /// 队列名称
        /// </summary>
        public string QueueName { get; set; }

        /// <summary>
        /// 队列信息
        /// </summary>
        public object QueueBody { get; set; }

        /// <summary>
        /// 当前时间
        /// </summary>
        public DateTime LogDate { get; set; }
    }
}
