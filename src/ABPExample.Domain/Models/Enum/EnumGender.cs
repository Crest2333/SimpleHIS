using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ABPExample.Domain.Models.Enum
{
    public enum EnumGender
    {
        [Description("其他")]
        Other,

        [Description("男")]
        Man,

        [Description("女")]
        Woman
    }
}
