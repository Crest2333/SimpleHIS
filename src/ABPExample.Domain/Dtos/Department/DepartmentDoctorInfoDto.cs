using System;
using System.Collections.Generic;
using System.Text;

namespace HIS.Domain.Dtos.Department
{
    public class DepartmentDoctorInfoDto
    {
        public List<string> DepartmentNameList { get; set; }

        public DoctorInfoDto DoctorInfo { get; set; }
    }

    public class DoctorInfoDto
    {
        public int DoctorId { get; set; }

        public string DoctorNo { get; set; }

        public string DoctorName { get; set; }

        public string ImgUrl { get; set; }

    }
}
