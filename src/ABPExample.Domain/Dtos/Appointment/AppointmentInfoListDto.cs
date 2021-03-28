using ABPExample.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using ABPExample.Domain.Common;

namespace ABPExample.Domain.Dtos.Appointment
{
    public class AppointmentInfoListDto
    {
        public int Id { get; set; }

        public string AppointmentNo { get; set; }

        public string PatientName { get; set; }

        public string PhoneNumber { get; set; }

        public string IdentityId { get; set; }

        public EnumGender Gender { get; set; }

        public string DoctorNo { get; set; }

        public string DoctorName { get; set; }

        public DateTime AppointmentDate { get; set; }

        public string Department { get; set; }

        public AppointmentStatusEnum Status { get; set; }

        public string StatusDesc => Status.GetDescription();

    }
}
