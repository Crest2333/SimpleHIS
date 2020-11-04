using ABPExample.Domain.Dtos.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ABPExample.Domain.Dtos.Drug
{
    public class DrugInfoSearchDto:PageSearchDto
    {
        public string Name { get; set; }
    }
}
