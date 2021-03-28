using System;
using System.Collections.Generic;
using System.Text;

namespace ABPExample.Domain.Dtos.MedicalAdvice
{
    public class MedicalAdviceInputDto
    {
        public int Id { get; set; }

        public int AppointmentId { get; set; }

        public string Content { get; set; }

    }
}
