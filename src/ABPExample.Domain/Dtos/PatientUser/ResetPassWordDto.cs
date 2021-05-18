using System;
using System.Collections.Generic;
using System.Text;

namespace HIS.Domain.Dtos.PatientUser
{
    public class ResetPassWordDto
    {
        public int Code { get; set; }

        public string Email { get; set; }

        public string  PassWord { get; set; }

    }
}
