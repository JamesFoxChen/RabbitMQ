using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using _11_MyMessage.Common;
using _11_MyMessage.Common.Error;
using _11_MyMessage.Common.Temp;

namespace _11.MyMessage.Services
{
    public abstract class ServiceBase<T>
    {
        protected void Consumer(string queueName)
        {
            var factory = new ConnectionFactory();
            factory.UserName = QueueSetttiong.UserName;   //用户名，对应Management工具的admin-->user
            factory.Password = QueueSetttiong.Password;   //密码，对应Management工具的admin-->密码
            factory.HostName = QueueSetttiong.HostName;   //本地部署服务直接用hostname即可
            factory.Port = AmqpTcpEndpoint.UseDefaultPort;
            factory.VirtualHost = QueueSetttiong.VirtualHost;   //使用默认值： "/"
            factory.Protocol = Protocols.DefaultProtocol;

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //定义队列
                    bool durable = true;
                    channel.QueueDeclare(queueName, durable, false, false, null);

                    channel.BasicQos(0, QueueSetttiong.DefaultPrefetechCount, false);

                    var consumer = new QueueingBasicConsumer(channel);
                    //consumer启动确认机制，处理完成告知队列，队列再移除消息
                    bool noAck = false;
                    channel.BasicConsume(queueName, noAck, consumer);

                    while (true)
                    {
                        BasicDeliverEventArgs ea = null;
                        bool isDequeue = consumer.Queue.Dequeue(QueueSetttiong.DequeueTimeout, out ea);
                        if (isDequeue)
                        {
                            T body = default(T);
                            try
                            {
                                //throw new Exception("处理消息发生异常");
                                string message = Encoding.UTF8.GetString(ea.Body);
                                body = JsonConvert.DeserializeObject<T>(message);

                                this.ConsumerData(body);

                                //告诉队列处理已完成
                                channel.BasicAck(ea.DeliveryTag, false);

                                logForDebug(queueName, message, body);
                            }
                            catch (Exception ex) //异常信息写入错误队列
                            {
                                sendToErrorQueue(ex, body);
                            }
                        }
                        else
                        {
                            //队列中无数据
                            //记录日志，分析队列情况
                            //BasicDeliverEventArgs ea = consumer.Queue.Dequeue();
                            //writeLogToMongo(channel, ea);     
                        }
                    }
                }
            }
        }

        protected abstract void ConsumerData(object body);

        /// <summary>
        /// 排错用日志，需要对队列进行调试排错时可通过配置文件启动
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="message"></param>
        /// <param name="body"></param>
        private void logForDebug(string queueName, string message, T body)
        {
            if (QueueSetttiong.IsLog)
            {
                //日志.Error("获取信息（" + queueName + "-->Consumer）" + message);
            }

            if (QueueSetttiong.IsWriteTempQueue)
            {
                TempQueueMng.GetInstance().SendToQueue(
                                new TempQueueModel
                                {
                                    Type = Enum_QueueType.Publish,
                                    QueueName = queueName,
                                    QueueBody = body
                                });
            }
        }

        /// <summary>
        /// 发送信息到错误队列
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="body"></param>
        private void sendToErrorQueue(Exception ex, T body)
        {
            var model = new ErrorQueueModel
            {
                Type = Enum_QueueType.Consumer.ToString(),
                ErrMessage = ex.Message,
                ErrStackTrace = ex.StackTrace,
                QueueName = QueueName.LogQueue,
                QueueBody = body
            };

            ErrorQueueMng.GetInstance().SendToQueue(model);
        }
    }
}
