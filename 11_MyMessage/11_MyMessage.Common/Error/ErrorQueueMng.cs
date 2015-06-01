using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace _11_MyMessage.Common.Error
{
    public class ErrorQueueMng
    {
        /// <summary>
        /// 发送到队列
        /// </summary>
        /// <param name="msg"></param>
        public static void SendToErrorQueue(ErrorQueueModel msg)
        {
            string queueName = QueueName.Common_Error_Queue;

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
                        //properties.Headers = new Dictionary<string, object>();
                        properties.SetPersistent(true);

                        var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(msg));
                        channel.BasicPublish("", queueName, properties, body);
                    }
                }

                //throw new Exception("sf");
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

                //记日志 .Error("写入错误队列异常（ErrorQueueMng-->SendToErrorQueue）：" + JsonConvert.SerializeObject(logErr));
            }
        }
    }
}
