using ABPExample.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace ABPExample.Domain.Dtos.Patient
{
    public class PatientInfoListDto
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public EnumGender Gender { get; set; }

        public string PhoneNumber { get; set; }

        public string IdentityId { get; set; }


    }
}
