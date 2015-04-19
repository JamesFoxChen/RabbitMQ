using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//http://www.rabbitmq.com/tutorials/tutorial-three-dotnet.html

namespace _03_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                //声明Exchange，模式为fanout（日志模式）
                //客户端和服务端都需要定义
                channel.ExchangeDeclare("logs", "fanout");  

                var message = GetMessage(args);
                var body = Encoding.UTF8.GetBytes(message);

                //logs表示exchange的名称，如果设置""则表示使用默认的exchange
                //设置exchange名称后无需设置队列名称，在服务端exchange会和队列进行绑定
                //一个exchange绑定到多个队列，消息先发送到exchange，然后exchange再将消息分发给绑定的队列
                channel.BasicPublish("logs", "", null, body);
                Console.WriteLine(" [x] Sent {0}", message);
            }
        }

        private static string GetMessage(string[] args)
        {
            return ((args.Length > 0) ? string.Join(" ", args) : "info: Hello World!");
        }
    }
}
