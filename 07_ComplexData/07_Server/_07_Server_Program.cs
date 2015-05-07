using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using _07_Model;

namespace _07_Server
{
    class _07_Server_Program
    {
        static void Main(string[] args)
        {
            string queueName = "07";

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //定义队列（hello为队列名）
                    channel.QueueDeclare(queueName, false, false, false, null);

                    var consumer = new QueueingBasicConsumer(channel);
                    bool noAck = true;
                    channel.BasicConsume(queueName, noAck, consumer);

                    Console.WriteLine(DateTime.Now + " [*] Waiting for messages.");
                    while (true)
                    {
                        //接受客户端发送的消息并打印出来
                        var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();

                        var message = (TestModel)SerializableUtil.Deserialize(ea.Body);
                        Console.WriteLine(" [x] Received {0}", message);
                    }
                }
            }
        }
    }
}
