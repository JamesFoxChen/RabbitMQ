using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _08_Client
{
    class ApiUrl
    {
        private static string predix = "http://localhost:15672/api/";

        public static string GetOverView()
        {
            return predix + "overview";
        }

        public static string GetConnections()
        {
            return predix + "connections";
        }

        public static string GetChannels()
        {
            return predix + "channels";
        }

        #region Exchanges
        public static string GetExchanges()
        {
            return predix + "exchanges";
        }

        /// <summary>
        /// exchange发布消息（发送到exchange绑定的队列中）
        /// api地址是动态拼接的
        /// vhost：vhost名称，默认为/,转换过来就是%2F
        /// name：exchange的名称，这里名称是logs（需要先通过Management管理工具定义)
        /// </summary>
        /// <returns></returns>
        public static string PublishExchanges()
        {
            //return predix + "exchanges/vhost/name/publish";
            return predix + "exchanges/%2F/logs/publish";
        }
        #endregion

        #region Queues
        public static string GetQueues()
        {
            return predix + "queues";
        }

        public static string GetQueueByName(string queueName)
        {
            return predix + "queues/%2F/" + queueName;
        }
        #endregion

        #region Bindings
        public static string GetBindings()
        {
            return predix + "bindings" ;
        }
        #endregion

        public static string GetConsumers()
        {
            return predix + "consumers";
        }

        public static string GetVhosts()
        {
            return predix + "vhosts";
        }

        public static string GetUsers()
        {
            return predix + "users";
        }

        public static string GetParameters()
        {
            return predix + "parameters";
        }

        public static string GetPolicies()
        {
            return predix + "policies";
        }
    }
}
