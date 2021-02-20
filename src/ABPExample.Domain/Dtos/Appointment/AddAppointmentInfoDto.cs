using System;
using System.Collections.Generic;
using System.Text;

namespace ABPExample.Domain.Dtos.Appointment
{
    public class AddAppointmentInfoDto
    {
        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        public int DepartmentId { get; set; }

        public string Describe { get; set; }

        public DateTime AppointmentDate { get; set; }

        public string AppointmentTime { get; set; }


    }
}
