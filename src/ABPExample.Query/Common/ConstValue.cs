﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ABPExample.Query.Common
{
    public static class ConstValue
    {
        public static IReadOnlyCollection<string> AppointmentTime = new List<string>
        {
            "08:00", "08:20", "08:40",
            "09:00", "09:20", "09:40",
            "10:00", "10:20", "10:40",
            "11:00", "11:20", "11:40",
            "13:00", "13:20", "13:40",
            "14:00", "14:20", "14:40",
            "15:00", "15:20", "15:40",
            "16:00", "16:20", "16:40",
        };
    }
}
