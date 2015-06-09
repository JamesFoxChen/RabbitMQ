using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using _11_MyMessage.Common.Client;

namespace _11_MyMessage.Common.Error
{
    public class ErrorQueueMng : QueuePublishBase
    {
        #region 单例模式
        private static ErrorQueueMng instance = null;
        public static ErrorQueueMng GetInstance()
        {
            if (instance == null)
            {
                lock ("")
                {
                    if (instance == null)
                    {
                        instance = new ErrorQueueMng();
                    }
                }
            }
            return instance;
        }
        private ErrorQueueMng() { }
        #endregion


        /// <summary>
        /// 发送到队列
        /// </summary>
        /// <param name="msg"></param>
        public void SendToQueue(ErrorQueueModel msg)
        {
            string queueName = QueueName.Common_Error_Queue;
            this.InitChannel(queueName);
            this.LogText(queueName, msg);

            try
            {
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(msg));
                channel.BasicPublish("", queueName, properties, body);


                //throw new Exception("kll");
            }
            catch (Exception ex)
            {
                TextLoggingService.Error("发送消息异常(异常队列)（队列名：" + queueName + "消息：" + msg + "异常信息：" + ex.Message);
            }
        }
    }
}
