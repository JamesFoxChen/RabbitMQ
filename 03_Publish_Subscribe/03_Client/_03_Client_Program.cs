using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//http://www.rabbitmq.com/tutorials/tutorial-three-dotnet.html

namespace _03_Client
{
    class _03_Client_Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                //声明exchange，模式为fanout（广播模式）
                //该模式下启动多个服务端，客户端发送一个消息后，所有的服务端都可以接收到
                //客户端和服务端都需要定义exchange，且名称需要一致
                channel.ExchangeDeclare("logs", ExchangeType.Fanout);  

                var message = GetMessage(args);
                var body = Encoding.UTF8.GetBytes(message);

                //logs表示exchange的名称，如果设置""则表示使用默认的exchange
                //设置exchange名称后无需设置队列名称，在服务端exchange会和队列进行绑定
                //一个exchange绑定到多个队列，消息先发送到exchange，然后exchange再将消息分发给绑定的队列
                //客户端直接通过exchange发送消息，不用定义队列
                channel.BasicPublish("logs", "", null, body);
                Console.WriteLine(" [x] Sent {0}", message);
            }
        }

        private static string GetMessage(string[] args)
        {
            return ((args.Length > 0) ? string.Join(" ", args) : "info: Hello World!" + "Date:" + DateTime.Now.ToString());
        }
    }
}
