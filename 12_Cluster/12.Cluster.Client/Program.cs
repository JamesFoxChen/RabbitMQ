using System;
using System.Text;
using RabbitMQ.Client;

namespace _12.Cluster.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            factory.UserName = "admin";
            factory.Password = "123456";
            factory.Port = AmqpTcpEndpoint.UseDefaultPort;
            factory.VirtualHost = "/";
            factory.Protocol = Protocols.DefaultProtocol;
            
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //exchange类型设为direct，不适用fanout的广播模式
                    //该模式下：设置router后，启动多个服务端，每个服务端绑定不同的router值，发送消息时
                    //只会把消息发送到匹配router值的服务端
                    //客户端和服务端都需要设置
                    channel.ExchangeDeclare("MyExchange", "direct");

                    bool durable = true;
                    channel.QueueDeclare("MyQueue", durable, false, false, null);

                    string[] routerArray = new string[] { "route29", "route88"};
                    string routerName = routerArray[new Random().Next(0, 2)];
                    var message = routerName + " Send!";
                    var body = Encoding.UTF8.GetBytes(message);

                    //随机发送到一个router
                    channel.BasicPublish("MyExchange", routerName, null, body);
                    Console.WriteLine(" [x] Sent '{0}':'{1}'", routerName, message);
                }
            }
        }
    }
}
