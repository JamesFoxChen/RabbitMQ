using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _11_MyMessage.Common;
using _11_MyMessage.Common.QueueModel;

namespace _11.MyMessage.Services
{
    class LogService : ServiceBase<LogQueueModel>
    {
        public void Start()
        {
            //记日志 .Error("JwifiRoute.Message.LogServices：日志服务启动");
            this.Consumer(QueueName.LogQueue);
        }

        protected override void ConsumerData(object msg)
        {
            var body = msg as LogQueueModel;

            //获取队列的值并处理
        }
    }
}
