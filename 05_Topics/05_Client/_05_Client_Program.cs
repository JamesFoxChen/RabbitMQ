using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _05_Client
{
    class _05_Client_Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //定义topic类型的exchange，用于多个条件的匹配判断
                    //该模式下：可匹配多个router
                    //客户端和服务端都需要定义
                    channel.ExchangeDeclare("topic_logs", "topic");

                    //下面两种routingKey，服务端都可以捕获到
                    //var routingKey = "quick.orange.rabbit";
                    var routingKey = "lazy.11.22";

                    var message = "topic exchange: Hello World!";
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish("topic_logs", routingKey, null, body);
                    Console.WriteLine(" [x] Sent '{0}':'{1}'", routingKey, message);
                }
            }
        }
    }
}
