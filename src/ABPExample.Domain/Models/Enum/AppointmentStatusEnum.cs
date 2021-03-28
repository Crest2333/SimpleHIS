using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ABPExample.Domain.Models.Enum
{
    public  enum AppointmentStatusEnum
    {

        [Description("已预约")]
        Reserved,

        [Description("已报道")]
        HasBeenReported,

        [Description("就诊中")]
        Visiting,

        [Description("已就诊")]
        Visited,

        [Description("已取消")]
        Cancelled,

        [Description("未报道")]
        NoReport

    }
}
