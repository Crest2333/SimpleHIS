using System;
using System.Collections.Generic;
using System.Text;
using ABPExample.Domain.Dtos.Common;

namespace ABPExample.Domain.Dtos.Scheduling
{
    public class GetSchedulingInputDto:PageSearchDto
    {
        public string Name { get; set; }

        public string UseNo { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? DepartmentId { get; set; }

    }
}
