using ABPExample.Domain.Public;
using System;
using System.Collections.Generic;
using System.Text;
using ABPExample.Domain.Models.Enum;

namespace ABPExample.Domain.Models
{
    public class Appointment : FullEntity
    {
        /// <summary>
        ///     预约号
        /// </summary>
        public string AppointmentNo { get; set; }

        public int PatientId { get; set; }

        public string DoctorNo { get; set; }

        public int DoctorId { get; set; }

        public string AppointmentTime { get; set; }

        public int DepartmentId { get; set; }

        public DateTime AppointmentDate { get; set; }

        public AppointmentStatusEnum Status { get; set; }

        public string Describe { get; set; }

    }
}
