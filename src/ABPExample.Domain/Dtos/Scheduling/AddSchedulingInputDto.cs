using System;
using System.Collections.Generic;
using System.Text;

namespace ABPExample.Domain.Dtos.Scheduling
{
    public class AddSchedulingInputDto
    {
        public string UserNo { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int SchedulingType { get; set; }

        public int  DepartmentId { get; set; }


    }
}
