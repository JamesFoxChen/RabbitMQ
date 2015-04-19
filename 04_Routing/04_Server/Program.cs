using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//http://www.rabbitmq.com/tutorials/tutorial-four-dotnet.html
//For example we may want the script which is writing log messages to the disk to only receive critical errors, 
//and not waste disk space on warning or info log messages.
//服务端绑定特定的RoutingKey，可以一个或多个；客户端绑定特定的RoutingKey
//客户端发送时只将消息发送到匹配RoutingKey的服务端
namespace _04_Server
{
    class Program
    {
        static void Main(string[] args)
        {

            //var r = new Random().Next(0, 3);
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //exchange类型设为direct，不适用fanout的广播模式
                    //客户端和服务端都需要设置
                    channel.ExchangeDeclare("direct_logs", "direct");

                    //随机声明队列名称
                    var queueName = channel.QueueDeclare().QueueName;

                    //if (args.Length < 1)
                    //{
                    //    Console.Error.WriteLine("Usage: {0} [info] [warning] [error]",
                    //                            Environment.GetCommandLineArgs()[0]);
                    //    Environment.ExitCode = 1;
                    //    return;
                    //}

                    //string[] argsSeverity = new string[] { "info", "warning", "error" };
                    string[] argsSeverity = new string[] { "info"};
                    foreach (var severity in argsSeverity)
                    {
                        channel.QueueBind(queueName, "direct_logs", severity);
                    }

                    Console.WriteLine(" [*] Waiting for messages. " +
                                      "To exit press CTRL+C");

                    var consumer = new QueueingBasicConsumer(channel);
                    channel.BasicConsume(queueName, true, consumer);

                    while (true)
                    {
                        var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();

                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        var routingKey = ea.RoutingKey;
                        Console.WriteLine(" [x] Received '{0}':'{1}'",
                                          routingKey, message);
                    }
                }
            }
        }
    }
}
