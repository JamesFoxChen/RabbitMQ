using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _11_MyMessage.Common.Error
{
    public class ErrorQueueModel
    {
        public ErrorQueueModel()
        {
            LogDate = DateTime.Now;
        }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrMessage { get; set; }

        /// <summary>
        /// 错误堆栈信息
        /// </summary>
        public string ErrStackTrace { get; set; }

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
