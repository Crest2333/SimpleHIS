using System;
using System.Collections.Generic;
using System.Text;

namespace ABPExample.Domain.Dtos.MedicalHistory
{
    public class AddPastHistoryDto
    {
        public string Name { get; set; }

        public int PatientId { get; set; }

        public string Describe { get; set; }

        public DateTime StartDate { get; set; }

    }
}
