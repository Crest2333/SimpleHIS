using System;
using System.Collections.Generic;
using System.Text;

namespace ABPExample.Domain.Dtos.Appointment
{
    public class EditAppointmentInputDto
    {
        public int AppointmentId { get; set; }

        public int Status { get; set; }

    }
}
