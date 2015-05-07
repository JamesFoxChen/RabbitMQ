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
    class _04_Server_Program
    {
        static void Main(string[] args)
        {

            //var r = new Random().Next(0, 3);
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //direct_logs为exchange名称
                    //exchange类型设为direct，不适用fanout的广播模式
                    //direct要通过router传播，fanout不用通过router，直接通过广播
                    //客户端和服务端都需要设置
                    channel.ExchangeDeclare("direct_logs", "direct");

                    //随机声明队列名称
                    var queueName = channel.QueueDeclare().QueueName;

                    //exhange、队列、router三个条件同时匹配，服务端才会接受到消息
                    string[] routerArray = new string[] { "router1", "router2", "router3" };
                    string routerName = routerArray[new Random().Next(0, 3)];
                    channel.QueueBind(queueName, "direct_logs", routerName);

                    Console.WriteLine(" Server Router Name:" + routerName);
                    Console.WriteLine(" [*] Waiting for messages. " +
                                      "To exit press CTRL+C");

                    var consumer = new QueueingBasicConsumer(channel);
                    channel.BasicConsume(queueName, true, consumer);

                    while (true)
                    {
                        var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();

                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        message = "QueneName:" + queueName + "   " + message;  //队列名+消息

                        var routingKey = ea.RoutingKey;
                        Console.WriteLine(" [x] Received '{0}':'{1}'",
                                          routingKey, message);
                    }
                }
            }
        }
    }
}
