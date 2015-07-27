using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using _11_MyMessage.Common;

namespace _11.MyMessage.Services
{
    public class TopshelfService
    {
        public void Start()
        {
            try
            {
                logServcieStart();
            }
            catch (Exception ex)
            {
                TextLoggingService.Error("服务运行异常（TopshelfService-->Start）" + ex.Message);
            }
        }

        private void logServcieStart()
        {
            Thread t = new Thread(new ThreadStart(new LogService().Start));
            t.Name = "LogServcie";
            t.Start();
        }


        public void Stop()
        {
        }
    }
}
