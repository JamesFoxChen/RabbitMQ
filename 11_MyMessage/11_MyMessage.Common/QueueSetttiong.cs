using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using System.Configuration;

namespace _11_MyMessage.Common
{
    public class QueueSetttiong
    {
        /// <summary>
        /// 标记数据来源于队列（用于具体业务） 
        /// </summary>
        public const int QueueSourceFlag = 1;

        /// <summary>
        /// 队列默认预取记录数量
        /// </summary>
        public const int DefaultPrefetechCount = 3;

        /// <summary>
        /// 从队列取数据超时时间
        /// </summary>
        public const int DequeueTimeout = 10000;

        /// <summary>
        ///  队列HostName
        /// </summary>
        public static string HostName
        {
            get
            {
                object value = ConfigurationManager.AppSettings["QueueHostName"];
                if (value == null)
                {
                    //记日志： .Error("QueueHostName配置文件读取失败，可能是无此配置文件");
                    throw new ArgumentNullException("QueueHostName");
                }
                return ConfigurationManager.AppSettings["QueueHostName"];
            }
        }


        /// <summary>
        /// 端口
        /// </summary>
        public static int Port
        {
            get { return AmqpTcpEndpoint.UseDefaultPort; }
        }

        /// <summary>
        /// 是否记录日志
        /// </summary>
        public static bool IsLog
        {
            get { return convertToBool("QueueIsLog"); }
        }


        /// <summary>
        /// 是否将服务队列获取的消息记入临时队列，方便测试
        /// </summary>
        public static bool IsWriteTempQueue
        {
            get { return convertToBool("QueueIsWriteTempQueue"); }
        }


        private static bool convertToBool(string keyName)
        {
            object value = ConfigurationManager.AppSettings[keyName];
            bool result = false;
            if (value == null || !bool.TryParse(value.ToString(), out result))
            {
                return false;
            }

            return Convert.ToBoolean(value);
        }


        /// <summary>
        /// 账号
        /// </summary>
        public static string UserName
        {
            get { return "guest"; }
        }

        /// <summary>
        /// 密码
        /// </summary>
        public static string Password
        {
            get { return "guest"; }
        }

        /// <summary>
        /// VirtualHost
        /// </summary>
        public static string VirtualHost
        {
            get { return "/"; }
        }
    }
}
