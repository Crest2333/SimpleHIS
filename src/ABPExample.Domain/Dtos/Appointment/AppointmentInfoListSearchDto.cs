using ABPExample.Domain.Dtos.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ABPExample.Domain.Dtos.Appointment
{
    public class AppointmentInfoListSearchDto: PageSearchDto
    {
        public int? PatientId { get; set; }

        public string PatientName { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string DoctorName { get; set; }

        public int DepartmentId { get; set; }

        public int? Status { get; set; }

    }
}
