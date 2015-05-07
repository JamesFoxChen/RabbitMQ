using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _07_Model;

namespace _07_Client
{
    class _07_Client_Program
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

                    //发送到队列的消息
                    var model = new TestModel
                    {
                        Id = 20,
                        Name = "James",
                        Brith = DateTime.Now,
                        Money = 1000
                    };

                    var body = SerializableUtil.Serialize(model);

                    channel.BasicPublish("", queueName, null, body);
                    Console.WriteLine("Sentd");
                }
            }
        }
    }
}
