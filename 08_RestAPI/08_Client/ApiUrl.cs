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

        public static string OverView()
        {
            return predix + "overview";
        }

        public static string Exchanges()
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
        
    }
}
