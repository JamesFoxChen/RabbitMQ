using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//http://www.rabbitmq.com/tutorials/tutorial-six-dotnet.html
//消息循环机制，从客户端发送到服务端，到最后消息回到客户端确认
//客户端发送一个消息，并附带唯一编号（CorrelationId）和确认队列(replyQueueName)
//服务端处理完成后将返回消息和唯一编号（CorrelationId）发送到确认队列(replyQueueName)
//客户端从确认队列(replyQueueName)读取消息并和唯一编号（CorrelationId）匹配后，获取服务端返回的消息
//Our RPC will work like this:
//When the Client starts up, it creates an anonymous exclusive callback queue.
//For an RPC request, the Client sends a message with two properties: replyTo, which is set to the callback queue and correlationId, which is set to a unique value for every request.
//The request is sent to an rpc_queue queue.
//The RPC worker (aka: server) is waiting for requests on that queue. When a request appears, it does the job and sends a message with the result back to the Client, using the queue from the replyTo field.
//The client waits for data on the callback queue. When a message appears, it checks the correlationId property. If it matches the value from the request it returns the response to the application.

//The server code is rather straightforward:

//As usual we start by establishing the connection, channel and declaring the queue.
//We might want to run more than one server process. In order to spread the load equally over multiple servers we need to set the prefetchCount setting in channel.basicQos.
//We use basicConsume to access the queue. Then we enter the while loop in which we wait for request messages, do the work and send the response back.

namespace _06_Server
{
    class _06_Server_Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //声明一个队列
                    string queueName = "rpc_queue";
                    channel.QueueDeclare(queueName, false, false, false, null);

                    //1表示队列每次只发送一个消息给consumer，直到consumer返回ack才发送下一个消息
                    //为了解决consumer负载过多问题
                    ushort prefetchCount = 1;
                    channel.BasicQos(0, prefetchCount, false);
                    var consumer = new QueueingBasicConsumer(channel);

                    //consumer启动确认机制，处理完成告知队列，队列再移除消息
                    bool noAck = false;
                    channel.BasicConsume(queueName, noAck, consumer);
                    Console.WriteLine(" [x] Awaiting RPC requests");

                    while (true)
                    {
                        string response = null;
                        var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();

                        var body = ea.Body;
                        //获取客户端设置的确认相关属性
                        var props = ea.BasicProperties;

                        //创建一个新的包含确认属性的对象
                        var replyProps = channel.CreateBasicProperties();
                        replyProps.CorrelationId = props.CorrelationId;
                        replyProps.ReplyTo = props.ReplyTo;

                        try
                        {
                            //获取客户端发送到队列的消息，并显示
                            var message = Encoding.UTF8.GetString(body);
                            //message = "QueneName:" + queueName + "   " + message;  //队列名+消息
                            Console.WriteLine(" [x] {0}", message);

                            //获取要发送到确认队列的消息
                            int n = int.Parse(message);
                            Console.WriteLine(" [.] fib({0})", message);
                            response = fib(n).ToString();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(" [.] " + e.Message);
                            response = "";
                        }
                        finally
                        {
                            var responseBytes = Encoding.UTF8.GetBytes(response);

                            //确认消息发送给确认队列(replyQueueName)，供客户端使用
                            channel.BasicPublish("", replyProps.ReplyTo, replyProps,
                                                responseBytes);
                            //channel.BasicPublish("", props.ReplyTo, replyProps,
                            //                     responseBytes);

                            //服务端发送确认消息到队列，以便从队列获取下一批消息
                            channel.BasicAck(ea.DeliveryTag, false);
                        }
                    }
                }
            }
        }

        private static int fib(int n)
        {
            if (n == 0 || n == 1) return n;
            return fib(n - 1) + fib(n - 2);
        }
    }
}
