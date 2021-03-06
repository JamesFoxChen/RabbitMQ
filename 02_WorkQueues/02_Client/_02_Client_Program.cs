﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _02_Client
{
    class _02_Client_Program
    {
        static void Main(string[] args)
        {
            string queueName = "02";

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //设置队列持久化，队列持久化意味着重启后，队列还是存在，但不等同于消息持久化,消息也需要设置持久化
                    //之前未设置队列持久化的队列，不能修改其持久化属性
                    //【客户端和服务端都需要设置持久化】
                    bool durable = true;
                    var dicArgs = new Dictionary<string, object>();
                    dicArgs.Add("x-message-ttl", 10000);
                    dicArgs = null;
                    channel.QueueDeclare(queueName, durable, false, false, dicArgs);

                    var message = GetMessage(args);
                    var body = Encoding.UTF8.GetBytes(message);

                    var properties = channel.CreateBasicProperties();
                    properties.Headers = new Dictionary<string,object>();

                    //设置消息的TTL11过期时间，而不是队列
                    //设置队列TTL时：定期从队头开始扫描是否有过期消息即可，队列中的消息TTL时间都一样
                    //设置消息TTL时(消息过期，也不会马上从队列中删除)：
                    //每条消息的过期时间不同，如果要删除所有过期消息，势必要扫描整个队列，
                    //所以等到此消息即将被消费时再判定是否过期(消息到队列头)，如果过期，再进行删除。

                    //properties.Expiration = "20000";   
                    
                    //客户端设置消息持久化(队列持久化和消息持久化必须同时设置）
                    //properties.DeliveryMode = 2;  等价于SetPersistent
                    properties.SetPersistent(true);

                    channel.BasicPublish("", queueName, properties, body);
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
