using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class _01_Server_Program
    {
        private static string queueName = "01";

        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //定义队列（hello为队列名）
                    channel.QueueDeclare(queueName, false, false, false, null);

                    ushort prefetechCount = 1;
                    channel.BasicQos(0, prefetechCount, false);

                    var consumer = new QueueingBasicConsumer(channel);
                    channel.BasicConsume(queueName, true, consumer);

                    Console.WriteLine(" [*] Waiting for messages." +
                                             "To exit press CTRL+C");

                    var sw = new Stopwatch();
                    sw.Start();

                    while (true)
                    {
                        //接受客户端发送的消息并打印出来
                        //var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();

                        BasicDeliverEventArgs ea = null;
                        consumer.Queue.Dequeue(2000, out ea);

                        if (ea == null)
                        {
                            sw.Stop();

                            TimeSpan ts = sw.Elapsed;
                            string elapsedTime = String.Format("{0:00}:时 {1:00}:分 {2:00}:秒：{3:00}:毫秒",
                                                                 ts.Hours, ts.Minutes, ts.Seconds,
                                                                 ts.Milliseconds / 10);

                            Console.WriteLine(" [x] Consumer Complete.Total Time:{0}", elapsedTime);
                            Console.ReadLine();
                        }

                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        //Console.WriteLine(" [x] Received {0}", message);

                        //Thread.Sleep(5000);
                    }
                }
            }
        }
    }
}
