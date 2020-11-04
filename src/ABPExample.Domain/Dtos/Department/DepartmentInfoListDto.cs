using System;
using System.Collections.Generic;
using System.Text;

namespace ABPExample.Domain.Dtos.Department
{
    public class DepartmentInfoListDto
    {
        public string Name { get; set; }

        public long Id { get; set; }

        public int PersonnelCount { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
