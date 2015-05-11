using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;
using _09_EasyNetQ_Message;

namespace _09_EasyNetQ_Server
{
    class Subscriber
    {

        public static void Receive()
        {
            using (var bus = RabbitHutch.CreateBus("host=localhost"))
            {
                //subscriptionId相同时，publisher消息轮流发送到Server
                //test为队列后缀名
                //一个subscriptionId名称创建一个队列
                //bus.Subscribe<TextMessage>("test", HandleTextMessage);

                //subscriptionId相同时，publisher消息同时发送到Server
                //string str = DateTime.Now.ToString();
                //bus.Subscribe<TextMessage>(str, HandleTextMessage);

                AsyncSubscripe(bus);
                //var log = new NullLogger();
                //log.DebugWrite("{0}", "ssss");
                Console.WriteLine("Listening for messages. Hit <return> to quit.");
                Console.ReadLine();


            }
        }

        //异步接收消息
        private static void AsyncSubscripe(IBus bus)
        {
            //bus.SubscribeAsync<TextMessage>("subscribe_async_test", message =>
            //       new WebClient().DownloadStringTaskAsync(new Uri("http://www.cnblogs.com"))
            //                      .ContinueWith(task =>
            //                            Console.WriteLine("Received: '{0}', Downloaded: '{1}'",
            //                                message.Text,
            //                                task.Result)));

            ISubscriptionResult result = bus.SubscribeAsync<Message>("subscribe_async_test", message => Task.Factory.StartNew(() =>
            {
                // Perform some actions here
                // If there is a exception it will result in a task complete but task faulted which
                // is dealt with below in the continuation
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Got message: {0}", message.Text);
                Console.ResetColor();

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

        static void HandleTextMessage(Message textMessage)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Got message: {0}", textMessage.Text);
            Console.ResetColor();
        }

        //static Task<string> HandleTextMessageAsyc(TextMessage textMessage)
        //{
        //    return Task.FromResult<string>(textMessage.Text).ContinueWith<string>((task) =>
        //            {
        //                Console.ForegroundColor = ConsoleColor.Red;
        //                Console.WriteLine("Got message: {0}", task.Result);
        //                Console.ResetColor();
        //            });
        //}
    }
}
