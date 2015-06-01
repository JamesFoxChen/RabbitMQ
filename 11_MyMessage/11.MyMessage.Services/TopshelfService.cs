using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11.MyMessage.Services
{
    public class TopshelfService
    {
        public void Start()
        {
            try
            {
                new LogService().Start();
            }
            catch (Exception ex)
            {
                //记日志.Error("服务运行异常（LogService-->Start）" + ex.Message);
            }
        }

        public void Stop()
        {
        }
    }
}
