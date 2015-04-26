using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _05_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //定义topic类型的exchange，用于多个条件的匹配判断
                    //客户端和服务端都需要定义
                    channel.ExchangeDeclare("topic_logs", "topic");

                    var routingKey = "quick.orange.rabbit";
                    var message = "quick.orange.rabbit" + "Hello World!";
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish("topic_logs", routingKey, null, body);
                    Console.WriteLine(" [x] Sent '{0}':'{1}'", routingKey, message);
                }
            }
        }
    }
}
