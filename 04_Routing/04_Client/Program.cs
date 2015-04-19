using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _04_Client
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
                    //exchange类型设为direct，不适用fanout的广播模式
                    //客户端和服务端都需要设置
                    channel.ExchangeDeclare("direct_logs", "direct");

                    //var severity = (args.Length > 0) ? args[0] : "info";
                    //var message = (args.Length > 1)
                    //                ? string.Join(" ", args.Skip(1).ToArray())
                    //                : "Hello World!";
                    string[] argsSeverity = new string[] { "info", "warning", "error" };
                    string severity = argsSeverity[new Random().Next(0, 3)];
                    var message = severity + "Hello World!";
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish("direct_logs", severity, null, body);
                    Console.WriteLine(" [x] Sent '{0}':'{1}'", severity, message);
                }
            }
        }
    }
}
