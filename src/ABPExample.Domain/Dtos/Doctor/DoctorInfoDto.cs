using System;
using System.Collections.Generic;
using System.Text;

namespace ABPExample.Domain.Dtos.Doctor
{
   public class DoctorInfoDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime WorkStartDate { get; set; }

        //public int Status { get; set; }

        //public int Grade { get; set; }

        public string Gender { get; set; }

        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        /// <summary>
        ///     工号
        /// </summary>
        public string WorkNo{ get; set; }
    }
}
