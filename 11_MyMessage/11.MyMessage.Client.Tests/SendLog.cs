using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using _11_MyMessage.Common.QueueModel;

namespace _11.MyMessage.Client.Tests
{
    [TestClass]
    public class SendLog
    {
        private int count = 10000;

        [TestMethod]
        public void 发送全局日志_连接初始化一次()
        {
            var sw = new Stopwatch();
            sw.Start();

            for (int i = 0; i < count; i++)
            {
                var msg = new LogQueueModel
                {

                    RequestId = "RequestId info",
                    RequestMsg = "RequestMsg info",
                    ResponseMsg = "ResponseMsg info",
                    Date = DateTime.Now
                };
                LogSend.GetInstance().SendToQueue(msg);
            }
            sw.Stop();

            TimeSpan ts = sw.Elapsed;
            string elapsedTime = String.Format("{0:00}:时 {1:00}:分 {2:00}:秒：{3:00}:毫秒",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);

            //00:时 00:分 00:秒：22:毫秒(1000)

            Assert.AreEqual(true, true);
        }
    }
}

