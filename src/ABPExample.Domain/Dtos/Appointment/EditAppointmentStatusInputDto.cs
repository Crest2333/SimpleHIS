using System;
using System.Collections.Generic;
using System.Text;
using ABPExample.Domain.Models.Enum;

namespace HIS.Domain.Dtos.Appointment
{
    public class EditAppointmentStatusInputDto
    {
        public int AppointmentId { get; set; }

        public AppointmentStatusEnum Status { get; set; }
    }
}
