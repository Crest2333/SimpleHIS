using ABPExample.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using ABPExample.Domain.Models;

namespace ABPExample.Domain.Dtos.Patient
{
    public class EditPatientInfoDto
    {

        public long Id { get; set; }

        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public string IdentityId { get; set; }

        public Gender Gender { get; set; }

        public decimal? Height { get; set; }

        public decimal? Weight { get; set; }

        public string Address { get; set; }
        public string BloodType { get; set; }
    }
}
