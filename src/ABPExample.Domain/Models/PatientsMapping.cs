using System;
using System.Collections.Generic;
using System.Text;
using ABPExample.Domain.Public;

namespace HIS.Domain.Models
{
    public class PatientsMapping : FullEntity
    {
        public int PatientId { get; set; }

        public int UserId { get; set; }

    }
}
