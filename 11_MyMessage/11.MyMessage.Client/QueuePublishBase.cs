using _11_MyMessage.Common;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _11_MyMessage.Common.Temp;

namespace _11.MyMessage.Client
{
    public class QueuePublishBase //<T> where T:new()
    {
        #region 属性

        protected IConnection connection;
        protected IModel channel;
        protected IBasicProperties properties;

        /// <summary>
        /// 队列名称
        /// </summary>
        protected string queueName;

        #endregion

        /// <summary>
        /// 初始化队列
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="msg"></param>
        protected void InitChannel(string queueName, object msg)
        {
            this.queueName = queueName;

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
                this.channel.QueueDeclare(this.queueName, durable, false, false, null);

                //持久化
                this.properties = channel.CreateBasicProperties();
                //properties.Headers = new Dictionary<string, object>();
                properties.SetPersistent(true);

                //LocalLoggingService.Error(this.connection.ToString());
            }

            if (QueueSetttiong.IsLog)
            {
                //记日志 .Error("发送信息（" + this.queueName + "-->Publish）" + msg);
            }

            if (QueueSetttiong.IsWriteTempQueue)
            {
                TempQueueMng.SendToTempQueue(Enum_QueueType.Publish, this.queueName, msg);
            }
        }
    }
}
