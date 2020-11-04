using System;
using System.Collections.Generic;
using System.Text;

namespace ABPExample.Domain.Dtos.PastHistory
{
    public class AddPastHistoryDto
    {
        public string Name { get; set; }

        public long PatientId { get; set; }

        public string Describe { get; set; }

    }
}
