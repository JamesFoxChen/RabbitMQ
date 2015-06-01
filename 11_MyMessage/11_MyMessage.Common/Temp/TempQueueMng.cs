using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using _11_MyMessage.Common.Error;

namespace _11_MyMessage.Common.Temp
{
    public class TempQueueMng
    {

        /// <summary>
        /// 发送到队列
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="queueName">队列名称</param>
        /// <param name="queueBody">队列内容</param>
        public static void SendToTempQueue(Enum_QueueType type, string queueName, object queueBody)
        {
            sendToTempQueue(new TempQueueModel
            {
                Type = type,
                QueueName = queueName,
                QueueBody = queueBody
            });
        }

        /// <summary>
        /// 发送到队列
        /// </summary>
        /// <param name="msg"></param>
        private static void sendToTempQueue(TempQueueModel msg)
        {
            string queueName = QueueName.Common_Temp_Queue;

            try
            {
                var factory = new ConnectionFactory() { HostName = QueueSetttiong.HostName };
                using (IConnection connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        //持久化
                        bool durable = true;
                        channel.QueueDeclare(queueName, durable, false, false, null);

                        //持久化
                        var properties = channel.CreateBasicProperties();
                        properties.SetPersistent(true);

                        var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(msg));
                        channel.BasicPublish("", queueName, properties, body);
                    }
                }
            }
            catch (Exception ex)
            {
                //记录文本日志
                //string exMsg = "写入错误队列异常";
                var logErr = new ErrorQueueModel();
                logErr.ErrMessage = ex.Message;
                logErr.ErrStackTrace = ex.StackTrace;
                logErr.QueueBody = msg.QueueBody;
                logErr.QueueName = msg.QueueName;

                //记日志 .Error("写入临时队列异常（ErrorQueueMng-->SendToErrorQueue）：" + JsonConvert.SerializeObject(logErr));
            }
        }
    }
}
