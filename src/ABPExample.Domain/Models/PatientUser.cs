using System;
using System.Collections.Generic;
using System.Text;
using ABPExample.Domain.Public;

namespace HIS.Domain.Models
{
    public class PatientUser:FullEntity
    {

        public string UserName { get; set; }

        public string PhoneNumber { get; set; }

        public string UserPwd { get; set; }

        public string Email { get; set; }

        public string UserImg { get; set; }

        public void Init()
        {
            UserImg = "Default.png";
            UserName = PhoneNumber;

        }

    }
}
