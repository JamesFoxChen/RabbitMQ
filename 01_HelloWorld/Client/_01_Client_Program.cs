using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class _01_Client_Program
    {
        private static string queueName = "01";
        private static int count = 1;
        private static int wrapCount = 1000;

        private static void Main(string[] args)
        {
            var sw = new Stopwatch();
            sw.Start();

            for (int j = 0; j < wrapCount; j++)
            {
                var factory = new ConnectionFactory() {HostName = "localhost"};
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        //定义队列（hello为队列名）
                        channel.QueueDeclare(queueName, false, false, false, null);

                        //发送到队列的消息，包含时间戳
                        string message = "Hello World!" + "_" + DateTime.Now.ToString();
                        var body = Encoding.UTF8.GetBytes(message);

                        for (int i = 0; i < count; i++)
                        {
                            channel.BasicPublish("", queueName, null, body);
                        }
                    }

                }
            }
            sw.Stop();

                TimeSpan ts = sw.Elapsed;
                string elapsedTime = String.Format("{0:00}:时 {1:00}:分 {2:00}:秒：{3:00}:毫秒",
                    ts.Hours, ts.Minutes, ts.Seconds,
                    ts.Milliseconds / 10);

                Console.WriteLine(" Count:{0} Producer Complete.Total Time:{1}", count, elapsedTime);
                Console.ReadLine();
        }
    }
}
