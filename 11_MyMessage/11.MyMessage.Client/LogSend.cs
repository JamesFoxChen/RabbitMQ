using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _11_MyMessage.Common;
using _11_MyMessage.Common.QueueModel;
using Newtonsoft.Json;

namespace _11.MyMessage.Client
{
    public class LogSend : QueuePublishBase
    {
        #region 单例模式
        private static LogSend instance = null;
        public static LogSend GetInstance()
        {
            if (instance == null)
            {
                lock ("")
                {
                    if (instance == null)
                    {
                        instance = new LogSend();
                    }
                }
            }
            return instance;
        }
        private LogSend() { }
        #endregion

        /// <summary>
        /// 发送到队列
        /// </summary>
        /// <param name="msg"></param>
        public void SendToQueue(LogQueueModel msg)
        {
            InitChannel(QueueName.LogQueue, msg);

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(msg));
            channel.BasicPublish("", queueName, properties, body);
        }

     
    }
}
