using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _04_Client
{
    class _04_Client_Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //exchange类型设为direct，不适用fanout的广播模式
                    //客户端和服务端都需要设置
                    channel.ExchangeDeclare("direct_logs", "direct");

                    string[] routerArray = new string[] { "router1", "router2", "router3" };
                    string routerName = routerArray[new Random().Next(0, 3)];
                    var message = routerName + " Send!";
                    var body = Encoding.UTF8.GetBytes(message);

                    //随机发送到一个router
                    channel.BasicPublish("direct_logs", routerName, null, body);
                    Console.WriteLine(" [x] Sent '{0}':'{1}'", routerName, message);
                }
            }
        }
    }
}
