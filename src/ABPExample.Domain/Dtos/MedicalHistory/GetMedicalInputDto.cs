using System;
using System.Collections.Generic;
using System.Text;

namespace ABPExample.Domain.Dtos.MedicalHistory
{
    public class GetMedicalInputDto
    {
        public int PatientId { get; set; }

        public int PageIndex { get; set; }

        public int  PageSize { get; set; }

    }
}
