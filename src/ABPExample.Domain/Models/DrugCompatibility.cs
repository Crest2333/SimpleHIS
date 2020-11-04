using ABPExample.Domain.Public;
using System;
using System.Collections.Generic;
using System.Text;

namespace ABPExample.Domain.Models
{
    public class DrugCompatibility:FullEntity
    {
        public int InDrugId { get; set; }

        public int OnDrugId { get; set; }

    }
}
