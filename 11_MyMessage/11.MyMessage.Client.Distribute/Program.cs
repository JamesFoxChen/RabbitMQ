using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace _11.MyMessage.Client.Distribute
{
    class Program
    {
        static void Main(string[] args)
        {
            //var factory = new ConnectionFactory();
            //factory.UserName = QueueSetttiong.UserName;
            //factory.Password = QueueSetttiong.Password;
            //factory.HostName = QueueSetttiong.HostName;
            //factory.Port = AmqpTcpEndpoint.UseDefaultPort;
            //factory.VirtualHost = QueueSetttiong.VirtualHost;
            //factory.Protocol = Protocols.DefaultProtocol;

            //this.connection = factory.CreateConnection();
            //this.channel = connection.CreateModel();

            ////持久化
            //bool durable = true;
            //this.channel.QueueDeclare(queueName, durable, false, false, null);

            ////持久化
            //this.properties = channel.CreateBasicProperties();
            ////properties.Headers = new Dictionary<string, object>();
            //properties.SetPersistent(true);
        }
    }
}
