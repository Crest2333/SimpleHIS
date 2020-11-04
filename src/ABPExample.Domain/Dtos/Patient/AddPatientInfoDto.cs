using ABPExample.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace ABPExample.Domain.Dtos.Patient
{
    public class AddPatientInfoDto
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
