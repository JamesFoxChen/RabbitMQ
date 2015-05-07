using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//http://www.rabbitmq.com/tutorials/tutorial-five-dotnet.html
//We created three bindings: Q1 is bound with binding key "*.orange.*" and Q2 with "*.*.rabbit" and "lazy.#".
//These bindings can be summarised as:
//Q1 is interested in all the orange animals.
//Q2 wants to hear everything about rabbits, and everything about lazy animals.
//A message with a routing key set to "quick.orange.rabbit" will be delivered to both queues. Message "lazy.orange.elephant" also will go to both of them. On the other hand "quick.orange.fox" will only go to the first queue, and "lazy.brown.fox" only to the second. "lazy.pink.rabbit" will be delivered to the second queue only once, even though it matches two bindings. "quick.brown.fox" doesn't match any binding so it will be discarded
//启动服务端（demo中启动了一个，实际需要启动三个），服务端绑定 "*.orange.*", "*.*.rabbit", "lazy.#"三种RoutingKey
//客服端发送消息的RoutingKey是quick.orange.rabbit，匹配"*.orange.*","*.*.rabbit"，这两个服务端都会接收到消息
namespace _05_Server
{
    class _05_Server_Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //定义topic类型的exchange，用于多个条件的匹配判断
                    channel.ExchangeDeclare("topic_logs", "topic");

                    //随机声明队列名称
                    var queueName = channel.QueueDeclare();

                    //满足多个router条件（router不是固定的，而是可匹配的）
                    //"*.orange.*" and Q2 with "*.*.rabbit" and "lazy.#".
                    //逗号隔开的多种匹配条件
                    string[] argsSeverity = new string[] { "*.orange.*", "*.*.rabbit", "lazy.#" };
                    foreach (var bindingKey in argsSeverity)
                    {
                        channel.QueueBind(queueName, "topic_logs", bindingKey);
                    }

                    Console.WriteLine(DateTime.Now.ToString() + " [*] Waiting for messages. " +
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
                        Console.WriteLine(" [x] Received 'Router：{0}':'{1}'",
                                          routingKey, message);
                    }
                }
            }
        }
    }
}
