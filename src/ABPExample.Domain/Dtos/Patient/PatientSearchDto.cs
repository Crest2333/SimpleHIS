using System;
using System.Collections.Generic;
using System.Text;

namespace ABPExample.Domain.Dtos.Patient
{
    public class PatientSearchDto
    {
        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string IdentityId { get; set; }

        public int? Gender { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

    }
}
