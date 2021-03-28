using System;
using System.Collections.Generic;
using System.Text;

namespace ABPExample.Domain.Dtos.MedicalAdvice
{
    public class MedicalAdviceEntityDto
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string PatientName { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? EditDate { get; set; }

        public int DoctorId { get; set; }

        public string DoctorName { get; set; }

        public string DoctorNo { get; set; }

    }
}
