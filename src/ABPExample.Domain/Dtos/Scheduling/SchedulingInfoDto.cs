using System;
using System.Collections.Generic;
using System.Text;

namespace ABPExample.Domain.Dtos.Scheduling
{
    public class SchedulingInfoDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public string UserNo { get; set; }

        public DateTime StartDate { get; set; }

        public int SchedulingType { get; set; }

        public DateTime EndDate { get; set; }

        public string DepartmentName { get; set; }

        public string OprNo { get; set; }

        public string OprName { get; set; }

        public DateTime OprDate { get; set; }

        public int DepartmentId { get; set; }

    }
}
