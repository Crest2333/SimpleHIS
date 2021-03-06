﻿using System;
using System.Collections.Generic;
using System.Text;
using ABPExample.Domain.Common;
using ABPExample.Domain.Models;
using ABPExample.Domain.Models.Enum;

namespace ABPExample.Domain.Dtos.Appointment
{
    public class AppointmentInfoDetailDto
    {
        public int Id { get; set; }

        public string PatientName { get; set; }

        public  Gender Gender { get; set; }

        public string GenderDesc => Gender.GetDescription();


        public string DoctorNo { get; set; }

        public string DoctorName { get; set; }

        public DateTime AppointmentDate { get; set; }

        public string Department { get; set; }

        public AppointmentStatusEnum Status { get; set; }

        public string PhoneNumber { get; set; }

        public string IdentityId { get; set; }

        public decimal? Height { get; set; }

        public decimal? Weight { get; set; }

        public string Address { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string BloodType { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Describe { get; set; }
    }
}
