using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03_Server
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
                    //声明Exchange，模式为fanout（日志模式）
                    //客户端和服务端都需要定义
                    channel.ExchangeDeclare("logs", "fanout");  

                    //随机定义一个队列
                    //In the .NET client, when we supply no parameters to queueDeclare() we create a non-durable, 
                    //exclusive, autodelete queue with a generated name:
                    //At that point queueName contains a random queue name. For example it may look like amq.gen-JzTY20BRgKO-HjmUJj0wLg.
                    var queueName = channel.QueueDeclare().QueueName;

                    //exchange和队列进行绑定，exchange将消息发送给多个队列
                    channel.QueueBind(queueName, "logs", "");
                    var consumer = new QueueingBasicConsumer(channel);
                    channel.BasicConsume(queueName, true, consumer);

                    Console.WriteLine(" [*] Waiting for logs." +
                                      "To exit press CTRL+C");
                    while (true)
                    {
                        var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();

                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine(" [x] {0}", message);
                    }
                }
            }
        }
    }
}
