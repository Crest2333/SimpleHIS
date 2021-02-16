using System;
using System.Collections.Generic;
using System.Text;

namespace ABPExample.Domain.Dtos.MedicalHistory
{
    public class MedicalInfoDto
    {
        public string Name { get; set; }

        public string Describe { get; set; }

        public string CreateBy { get; set; }

        public DateTime? StartDate { get; set; }

    }
}
