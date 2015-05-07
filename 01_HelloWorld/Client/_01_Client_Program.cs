using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class _01_Client_Program
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

                    //发送到队列的消息，包含时间戳
                    string message = "Hello World!" + "_" + DateTime.Now.ToString();
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish("", queueName, null, body);
                    Console.WriteLine(" [x] Sent {0}", message);
                }
            }
        }
    }
}
