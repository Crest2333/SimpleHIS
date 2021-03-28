using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ABPExample.Domain.Public;

namespace ABPExample.Domain.Models
{
    public class MedicalAdvice:FullEntity
    {
        public string Content { get; set; }

        public int AppointmentId { get; set; }

        public int DoctorId { get; set; }

        [Required]
        [DefaultValue("")]
        public string DoctorNo { get; set; }

        public int PatientId { get; set; }

    }
}
