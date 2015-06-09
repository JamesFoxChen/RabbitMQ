using _11_MyMessage.Common;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _11_MyMessage.Common.Temp;

namespace _11_MyMessage.Common.Client
{
    public class QueuePublishBase //<T> where T:new()
    {
        #region 属性

        protected IConnection connection;
        protected IModel channel;
        protected IBasicProperties properties;
        #endregion

        /// <summary>
        /// 初始化队列，并根据实际情况记录文档日志、临时队列
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="msg"></param>
        protected void InitChannelWithLog(string queueName, object msg)
        {
            InitChannel(queueName);
            LogText(queueName, msg);
            LogTempQueue(queueName, msg);
        }

        /// <summary>
        /// 初始化队列
        /// </summary>
        /// <param name="queueName"></param>
        protected void InitChannel(string queueName)
        {
            //只在第一次发送队列时初始化，后续发送复用连接
            if (this.connection == null)
            {
                var factory = new ConnectionFactory();
                factory.UserName = QueueSetttiong.UserName;
                factory.Password = QueueSetttiong.Password;
                factory.HostName = QueueSetttiong.HostName;
                factory.Port = AmqpTcpEndpoint.UseDefaultPort;
                factory.VirtualHost = QueueSetttiong.VirtualHost;
                factory.Protocol = Protocols.DefaultProtocol;

                this.connection = factory.CreateConnection();
                this.channel = connection.CreateModel();

                //持久化
                bool durable = true;
                this.channel.QueueDeclare(queueName, durable, false, false, null);

                //持久化
                this.properties = channel.CreateBasicProperties();
                //properties.Headers = new Dictionary<string, object>();
                properties.SetPersistent(true);
            }
        }

        protected void LogText(string queueName,object msg)
        {
            if (QueueSetttiong.IsLog)
            {
                TextLoggingService.Error("发送信息（" + queueName + "-->Publish）" + msg);
            }
        }

        protected void LogTempQueue(string queueName, object msg)
        {
            if (QueueSetttiong.IsWriteTempQueue)
            {
                TempQueueMng.GetInstance().SendToQueue(
                    new TempQueueModel
                    {
                        Type = Enum_QueueType.Publish,
                        QueueName = queueName,
                        QueueBody = msg
                    });
            }
        }
    }
}
