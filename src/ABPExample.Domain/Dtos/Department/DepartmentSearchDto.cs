using System;
using System.Collections.Generic;
using System.Text;

namespace ABPExample.Domain.Dtos.Department
{
    public class DepartmentSearchDto
    {
        public string Name { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
