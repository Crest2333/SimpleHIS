﻿using ABPExample.Domain.Public;
using System;
using System.Collections.Generic;
using System.Text;

namespace ABPExample.Domain.Models
{
    public class Appointment:FullEntity
    {
        /// <summary>
        ///     预约号
        /// </summary>
        public string AppointmentNo { get; set; }

        public int PatientId { get; set; }

        public string DoctorNo { get; set; }

        public int DepartmentId { get; set; }

        public DateTime AppointmentDate { get; set; }

        public int Status { get; set; }

        public string Describe { get; set; }

    }
}
