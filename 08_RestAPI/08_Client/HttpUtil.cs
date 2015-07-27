using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace _08_Client
{
    class HttpUtil
    {
        public static string SendPost(string url, string body)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.Timeout = 200000;

            NetworkCredential nc = new NetworkCredential("admin", "123456");
            httpWebRequest.Credentials = nc;
            byte[] btBodys = Encoding.UTF8.GetBytes(body);
            httpWebRequest.ContentLength = btBodys.Length;
            httpWebRequest.GetRequestStream().Write(btBodys, 0, btBodys.Length);

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
            string responseContent = streamReader.ReadToEnd();

            httpWebResponse.Close();
            streamReader.Close();
            httpWebRequest.Abort();
            httpWebResponse.Close();

            return responseContent;
        }

        public static string SendGet(string url)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            httpWebRequest.Timeout = 200000;

            //string credentials = base64.b64encode(string.Format("%s:%s", "guest", "guest");
            //string credentials = string.Format("{0}:{1}", "guest", "guest");
            //httpWebRequest.Headers.Add("Authorization", "Basic " + credentials);

            //设置验证头
            NetworkCredential nc = new NetworkCredential("admin", "123456");
            httpWebRequest.Credentials = nc;

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
            string responseContent = streamReader.ReadToEnd();

            httpWebResponse.Close();
            streamReader.Close();
            httpWebRequest.Abort();
            httpWebResponse.Close();

            return responseContent;
        }


        public static T SendPostAndReturnObject<T>(string url, object requestEntity)
        {
            string body = requestEntity.ToJson();

            var result = HttpUtil.SendPost(url, body);
            var responseObj = result.ToObject<T>();
            return responseObj;
        }

    }
}
