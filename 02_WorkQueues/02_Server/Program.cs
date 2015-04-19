using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _02_Server
{
    //可以同时启动多个Worker（Consumer）；队列通过轮询方式将消息发送到不同的Worker（Consumer）
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //设置队列持久化，队列重启也没事；
                    //之前未设置队列持久化的队列，不能修改其持久化属性
                    //【客户端和服务端都需要设置持久化】
                    bool durable = true;
                    channel.QueueDeclare("task_queue", durable, false, false, null);

                    //1表示队列每次只发送一个消息给consumer，直到consumer返回ack才发送下一个消息
                    //为了解决consumer负载过多问题
                    channel.BasicQos(0, 1, false);
                    var consumer = new QueueingBasicConsumer(channel);

                    //consumer启动确认机制，处理完成告知队列，队列再移除消息
                    bool noAck = false;
                    channel.BasicConsume("task_queue", noAck, consumer);

                    Console.WriteLine(" [*] Waiting for messages. " +
                                      "To exit press CTRL+C");
                    while (true)
                    {
                        var ea =
                            (BasicDeliverEventArgs)consumer.Queue.Dequeue();

                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine(" [x] Received {0}", message);

                        int dots = message.Split('.').Length - 1;
                        Thread.Sleep(dots * 1000);

                        Console.WriteLine(" [x] Done");

                        channel.BasicAck(ea.DeliveryTag, false);
                    }
                }
            }
        }
    }
}
