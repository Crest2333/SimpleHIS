using System;
using System.Collections.Generic;
using System.Text;
using ABPExample.Domain.Models.Enum;

namespace ABPExample.Domain.Dtos.Appointment
{
    public class EditAppointmentInputDto
    {
        public int AppointmentId { get; set; }

        public AppointmentStatusEnum Status { get; set; }

    }
}
