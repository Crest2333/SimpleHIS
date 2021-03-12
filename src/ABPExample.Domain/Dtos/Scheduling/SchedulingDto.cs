using System;
using System.Collections.Generic;
using System.Text;

namespace ABPExample.Domain.Dtos.Scheduling
{
    public class SchedulingDto
    {
        public SchedulingDto()
        {
            AppointmentNum = new List<string>();
        }
        public string SchedulingDate { get; set; }

        public List<string> AppointmentNum { get; set; }
    }
}
