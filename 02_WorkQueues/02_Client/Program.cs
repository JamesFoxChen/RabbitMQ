using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _02_Client
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
                    //设置队列持久化，队列重启也没事；
                    //队列持久化意味着重启后，队列会再出现，但不等同于消息持久化
                    //之前未设置队列持久化的队列，不能修改其持久化属性
                    //【客户端和服务端都需要设置持久化】
                    bool durable = true;
                    channel.QueueDeclare("task_queue", durable, false, false, null);

                    var message = GetMessage(args);
                    var body = Encoding.UTF8.GetBytes(message);

                    var properties = channel.CreateBasicProperties();

                    //客户端设置消息持久化(队列持久化和消息持久化必须同时设置）
                    //properties.DeliveryMode = 2;  等价于SetPersistent
                    properties.SetPersistent(true);                     

                    channel.BasicPublish("", "task_queue", properties, body);
                    Console.WriteLine(" [x] Sent {0}", message);
                }
            }
        }

        private static string GetMessage(string[] args)
        {
            return ((args.Length > 0) ? string.Join(" ", args) : "Hello World!  " + DateTime.Now.ToString());
        }
    }
}
