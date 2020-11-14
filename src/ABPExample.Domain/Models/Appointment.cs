using ABPExample.Domain.Public;
using System;
using System.Collections.Generic;
using System.Text;

namespace ABPExample.Domain.Models
{
    public class Appointment:FullEntity
    {
        public int PatientId { get; set; }

        public string PatientName { get; set; }

        public string PhoneNumber { get; set; }

        public string DoctorNo { get; set; }

        public int DepartmentId { get; set; }

        public DateTime AppointmentDate { get; set; }

        public int Status { get; set; }

        public string Describe { get; set; }

    }
}
