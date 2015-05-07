using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//The client code is slightly more involved:

//We establish a connection and channel and declare an exclusive 'callback' queue for replies.
//We subscribe to the 'callback' queue, so that we can receive RPC responses.
//Our call method makes the actual RPC request.
//Here, we first generate a unique correlationId number and save it - the while loop will use this value to catch the appropriate response.
//Next, we publish the request message, with two properties: replyTo and correlationId.
//At this point we can sit back and wait until the proper response arrives.
//The while loop is doing a very simple job, for every response message it checks if the correlationId is the one we're looking for. If so, it saves the response.
//Finally we return the response back to the user.

namespace _06_Client 
{
    class _06_Client_Program
    {
        static void Main(string[] args)
        {
            var rpcClient = new RPCClient();

            Console.WriteLine(" [x] Requesting fib(30)");
            var response = rpcClient.Call("30"); //计算结果
            Console.WriteLine(" [.] Done '{0}'", response);

            rpcClient.Close();

            Console.ReadKey();
        }
    }

    class RPCClient
    {
        private IConnection connection;
        private IModel channel;
        private string replyQueueName;
        private QueueingBasicConsumer consumer;
        private string routingKey = "rpc_queue";

        public RPCClient()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            this.connection = factory.CreateConnection();
            this.channel = this.connection.CreateModel();
            //创建确认队列
            this.replyQueueName = this.channel.QueueDeclare("replyQueueName", false, false, false, null); 
            this.consumer = new QueueingBasicConsumer(this.channel);
            this.channel.BasicConsume(this.replyQueueName, true, this.consumer);
        }

        public string Call(string message)
        {
            var corrId = Guid.NewGuid().ToString();

            //创建确认相关属性
            var props = channel.CreateBasicProperties();
            props.ReplyTo = replyQueueName;
            props.CorrelationId = corrId;

            var messageBytes = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish("", this.routingKey, props, messageBytes);

            while (true)
            {
                var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
                if (ea.BasicProperties.CorrelationId == corrId)
                {
                    return Encoding.UTF8.GetString(ea.Body);
                }
            }

            return "";
        }

        public void Close()
        {
            connection.Close();
        }
    }
}
