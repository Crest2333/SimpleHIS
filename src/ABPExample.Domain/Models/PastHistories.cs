using ABPExample.Domain.Public;
using System;
using System.Collections.Generic;
using System.Text;

namespace ABPExample.Domain.Models
{
    public class PastHistories:FullEntity
    {
        public string Name { get; set; }

        public long PatientId { get; set; }

        public string Describe { get; set; }

        public string CreateBy { get; set; }

    }
}
