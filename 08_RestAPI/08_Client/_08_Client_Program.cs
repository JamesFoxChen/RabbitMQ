using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _08_Client
{
    class _08_Client_Program
    {
        private static string response;
        static void Main(string[] args)
        {

            //string response = HttpUtil.SendGet(ApiUrl.Exchanges());
            //response = HttpUtil.SendGet(ApiUrl.GetConnections());
            //response = HttpUtil.SendGet(ApiUrl.GetChannels());
            //response = HttpUtil.SendGet(ApiUrl.GetQueues());
            //response = HttpUtil.SendGet(ApiUrl.GetQueueByName("Log_GlobalRequestLogQueue"));
            //response = HttpUtil.SendGet(ApiUrl.GetConsumers());
            //response = HttpUtil.SendGet(ApiUrl.GetBindings());
            //response = HttpUtil.SendGet(ApiUrl.GetVhosts());
            //response = HttpUtil.SendGet(ApiUrl.GetUsers());
            //response = HttpUtil.SendGet(ApiUrl.GetParameters());
            //response = HttpUtil.SendGet(ApiUrl.GetPolicies());
        }

        /// <summary>
        /// 通过Exchange给绑定的队列发消息
        /// </summary>
        /// <returns></returns>
        private static void exchangeSendMsg()
        {
            string msg = "{\"properties\":{},\"routing_key\":\"logs\",\"payload\":\"my body\",\"payload_encoding\":\"string\"}";
            response = HttpUtil.SendPost(ApiUrl.PublishExchanges(), msg);
        }

        /// <summary>
        /// 获取系统信息
        /// </summary>
        /// <returns></returns>
        private static void getOverView()
        {
            response = HttpUtil.SendGet(ApiUrl.GetOverView());
        }
    }
}
//exchanges