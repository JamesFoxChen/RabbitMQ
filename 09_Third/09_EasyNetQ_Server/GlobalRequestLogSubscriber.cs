using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;
using Newtonsoft.Json;
using _09_EasyNetQ_Message;

namespace _09_EasyNetQ_Server
{
    class RequestLogSubscriber
    {
        public static void Receive()
        {
            using (var bus = RabbitHutch.CreateBus("host=localhost"))
            {
                //bus.Subscribe<RequestLog>(string.Empty, HandleMessage);
                AsyncSubscripe(bus);
                Console.WriteLine("Listening for messages. Hit <return> to quit.");
                Console.ReadLine();
            }
        }


        //异步接收消息
        private static void AsyncSubscripeBySBytes(IBus bus)
        {
            ISubscriptionResult result = bus.SubscribeAsync<RequestLog>("", model => Task.Factory.StartNew(() =>
            {
                //EasyNetQ.JsonSerializer s = new EasyNetQ.JsonSerializer(new TypeNameSerializer());

                //var model = s.BytesToMessage<RequestLog>(msg);
                //Console.ForegroundColor = ConsoleColor.Red;
                //Console.WriteLine("Input: {0}", model.RequestLog.Input);
                //Console.WriteLine("Output: {0}", model.RequestLog.Output);
                //Console.WriteLine("RequestDate: {0}", model.RequestLog.RequestDate.ToString());
                //Console.ResetColor();

                //throw new Exception("AsyncSubscripe Exception");
            }).ContinueWith(task =>
            {
                if (task.IsCompleted && !task.IsFaulted)  //处理正常
                {

                }
                else   //异常
                {
                    // Dont catch this, it is caught further up the heirarchy and results in being sent to the default error queue
                    // on the broker
                    //不用捕获此异常，异常信息会自动写入Queue EasyNetQ_Default_Error_Queue队列
                    //该队列包含了接收的消息和异常信息
                    throw new EasyNetQException("Message processing exception - look in the default error queue (broker)");
                }
            }));
        }


        //异步接收消息
        private static void AsyncSubscripe(IBus bus)
        {
            ISubscriptionResult result = bus.SubscribeAsync<RequestLog>("", model => Task.Factory.StartNew(() =>
            {
                //EasyNetQ.JsonSerializer s = new EasyNetQ.JsonSerializer(new TypeNameSerializer());

                ////var model = s.BytesToMessage<RequestLog>(msg);
                //Console.ForegroundColor = ConsoleColor.Red;
                //Console.WriteLine("Input: {0}", model.RequestLog.Input);
                //Console.WriteLine("Output: {0}", model.RequestLog.Output);
                //Console.WriteLine("RequestDate: {0}", model.RequestLog.RequestDate.ToString());
                //Console.ResetColor();

                //throw new Exception("AsyncSubscripe Exception");
            }).ContinueWith(task =>
            {
                if (task.IsCompleted && !task.IsFaulted)  //处理正常
                {

                }
                else   //异常
                {
                    // Dont catch this, it is caught further up the heirarchy and results in being sent to the default error queue
                    // on the broker
                    //不用捕获此异常，异常信息会自动写入Queue EasyNetQ_Default_Error_Queue队列
                    //该队列包含了接收的消息和异常信息
                    throw new EasyNetQException("Message processing exception - look in the default error queue (broker)");
                }
            }));
        }

        static void HandleMessage(RequestLog msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Input: {0}", msg.Input);
            Console.WriteLine("Output: {0}", msg.Output);
            Console.ResetColor();
        }
    }
}
