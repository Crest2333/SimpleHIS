using System;
using System.Collections.Generic;
using System.Text;
using ABPExample.Domain.Public;

namespace ABPExample.Domain.Models
{
    public class Scheduling:FullEntity
    {
        public int UserId { get; set; }

        public string UserNo { get; set; }

        public DateTime StartDate { get; set; }

        public int SchedulingType { get; set; }

        public DateTime EndDate { get; set; }

        public int DepartmentId { get; set; }

        public int OprId { get; set; }

        public string OprNo { get; set; }

        public DateTime OprDate { get; set; }

    }
}
