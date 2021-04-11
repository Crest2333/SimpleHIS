using System;
using System.Collections.Generic;
using System.Text;

namespace HIS.Domain.Dtos.PatientUser
{
   public  class PatientInputDto
    {
        public int Id { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string PassWord { get; set; }
    }
}
