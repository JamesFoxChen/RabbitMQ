using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Topshelf;

namespace _11.MyMessage.Services
{
    class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
            HostFactory.Run(c =>
            {
                c.SetServiceName("11.MyMessage.Services");
                c.SetDisplayName("11.MyMessage.Services");
                c.SetDescription("11.MyMessage.Services");

                c.Service<TopshelfService>(s =>
                {
                    s.ConstructUsing(b => new TopshelfService());
                    s.WhenStarted(o => o.Start());
                    s.WhenStopped(o => o.Stop());
                });
            });
        }
    }
}
