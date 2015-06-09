using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using _11_MyMessage.Common.Client;
using _11_MyMessage.Common.Error;

namespace _11_MyMessage.Common.Temp
{
    public class TempQueueMng : QueuePublishBase
    {

        #region 单例模式
        private static TempQueueMng instance = null;
        public static TempQueueMng GetInstance()
        {
            if (instance == null)
            {
                lock ("")
                {
                    if (instance == null)
                    {
                        instance = new TempQueueMng();
                    }
                }
            }
            return instance;
        }
        private TempQueueMng() { }
        #endregion

        /// <summary>
        /// 发送到队列
        /// </summary>
        /// <param name="msg"></param>
        public void SendToQueue(TempQueueModel msg)
        {
            string queueName = QueueName.Common_Temp_Queue;
            this.InitChannel(queueName);
            this.LogText(queueName, msg);

            try
            {
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(msg));
                channel.BasicPublish("", queueName, properties, body);
            }
            catch (Exception ex)
            {
                TextLoggingService.Error("发送消息异常(临时队列)（队列名：" + queueName + "消息：" + msg + "异常信息：" + ex.Message);
            }
        }
    }
}
