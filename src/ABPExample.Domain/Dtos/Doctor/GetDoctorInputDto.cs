using System;
using System.Collections.Generic;
using System.Text;

namespace ABPExample.Domain.Dtos.Doctor
{
   public  class GetDoctorInputDto
    {
        public int DepartmentId { get; set; }

        public string Name { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
