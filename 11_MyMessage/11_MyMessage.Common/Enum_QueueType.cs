using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace _11_MyMessage.Common
{
    public enum Enum_QueueType
    {
        [Description("Publish")]
        Publish = 1,

        [Description("Consumer")]
        Consumer = 2
    }
}
