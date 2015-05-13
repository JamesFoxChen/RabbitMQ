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
    class _02_Server_Program
    {
        private static string queueName = "02";
        private static int count = 0;

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
                    //？未指定router时，router名称和queueName一致;exchange名称为默认值：(AMQP default)
                    bool durable = true;

                    //设置队列ttl过期时间，producer和consumer都需要设置
                    //消息在队列中超过这个时间就会自动删除，可以结合Nack一起使用
                    var dicArgs = new Dictionary<string, object>();
                    dicArgs.Add("x-message-ttl", 10000);
                    dicArgs = null;
                    channel.QueueDeclare(queueName, durable, false, false, dicArgs);

                    //同时启动多个服务时，第1个服务一次获取3个消息(第1个即使没ack，也会取第2个消息)；第2个服务只能从第4个消息获取
                    //如果第1个服务处理完第1个消息后被关闭，
                    //那么后面的第2，3个消息处于未确认消息，仍然在队列中，第2个服务会自动取出第2,3个消息进行处理
                    //ushort prefetechCount = 3;
                    ushort prefetechCount = 1;

                    //prefetechCount表示服务端每次只取指定数量的消息，直到服务端发送ack（执行BasicAck）才获取下一批消息
                    //为了解决consumer负载过多问题
                    channel.BasicQos(0, prefetechCount, false);
                    var consumer = new QueueingBasicConsumer(channel);

                    //consumer启动确认机制，处理完成告知队列，队列再移除消息
                    bool noAck = false;
                    channel.BasicConsume(queueName, noAck, consumer);

                    Console.WriteLine(DateTime.Now.ToString() + " [*] Waiting for messages. ");
                    while (true)
                    {
                        ////判断队列，如果为空，则返回
                        // if (consumer.Queue.()) continue;
                        
                        //没有执行BasicAck时，第一次获取消息后，下次执行到此语句时就会一直处于阻塞状态
                        //第一次获取的消息状态也是Unacked
                        //执行BasicAck后（确认机制），每隔2秒就会从队列获取下一条消息
                        //BasicDeliverEventArgs ea = new BasicDeliverEventArgs();
                        //consumer.Queue.Dequeue(3000, out ea);  //队列无数据时，超过3秒钟，返回ea，ea为null

                        BasicDeliverEventArgs ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();  //队列无数据时，直接阻塞

                        var message = Encoding.UTF8.GetString(ea.Body);
                        message = "QueneName:" + queueName + "   " + message;  //队列名+消息
                        Console.WriteLine(" [x] Sleep Start! {0}", message);

                        Thread.Sleep(2000);

                        Console.WriteLine(" [x] Sleep Done!");

                        //默认发生业务异常后，将消息重新发到队列头，下次可以重新取出处理
                        //也可能该消息被其它服务端(Consumer)处理
                        if (true)
                        {
                            channel.BasicNack(ea.DeliveryTag, false, true);
                            //exceptionReDeliver(channel, ea);
                            //nack(channel, ea);
                        }
                        else
                        {
                            //服务端不向队列发送确认消息时，消息一直处于Unacked状态 
                            //服务端关闭后，处于Unacked状态的消息又会变为Ready状态
                            //？DeliveryTag=1，为什么等于1，1表示什么意思，未知
                            channel.BasicAck(ea.DeliveryTag, false);
                        }
                    }
                }
            }
        }

        private static void exceptionReDeliver(IModel channel, BasicDeliverEventArgs ea)
        {
            //服务端接收消息时，如果流程或代码异常，则"x-redelivered-count"+1，并重新发送到队列
            //如果"x-redelivered-count"大于某个数时，就不再发送到队列，直接确认即可，不再处理，或写入异常数据库
            string key = "x-redelivered-count";
            var properties = ea.BasicProperties;
            if (properties.Headers == null)
            {
                properties.Headers = new Dictionary<string, object>();
                ea.BasicProperties.Headers.Add(key, 0);
            }
            ea.BasicProperties.Headers[key] = Convert.ToInt32(ea.BasicProperties.Headers[key]) + 1;

            //未超过重试次数前，重新发送消息到队列中
            if (Convert.ToInt32(ea.BasicProperties.Headers[key]) < 3)  
            {
                channel.BasicPublish("", queueName, ea.BasicProperties, ea.Body);
            }
            channel.BasicAck(ea.DeliveryTag, false);
        }


        private static void nack(IModel channel, BasicDeliverEventArgs ea)
        {
            //DeliveryTag 每次发送Nack，该值都会+1
            //requeue设为false时，无法再从队列获取消息
            if (count < 3)
            {
                channel.BasicNack(ea.DeliveryTag, false, true);
                count++;
            }
            else  //超过重新次数后，不再重试
            {
                count = 0;
                //ea.DeliveryTag = 1;  //设置该属性会导致SharedQueue closed异常
                channel.BasicAck(ea.DeliveryTag, false);
            }
        }
    }
}
