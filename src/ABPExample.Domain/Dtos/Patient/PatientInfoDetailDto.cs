using System;
using System.Collections.Generic;
using System.Text;
using ABPExample.Domain.Models.Enum;

namespace ABPExample.Domain.Dtos.Patient
{
    public class PatientInfoDetailDto
    {
        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public string IdentityId { get; set; }

        public EnumGender Gender { get; set; }

        public decimal? Height { get; set; }

        public decimal? Weight { get; set; }

        public string Address { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string BloodType { get; set; }
    }
}
