using System;
using System.Collections.Generic;
using System.Text;

namespace HIS.Domain.Dtos.Doctor
{
   public class DoctorEntityDto
   {
      public int Id { get; set; }

      public string Name { get; set; }

      public DateTime WorkStartDate { get; set; }


      public string Gender { get; set; }

      public List<string> DepartmentList { get; set; }

      /// <summary>
      ///     工号
      /// </summary>
      public string WorkNo { get; set; }

      public string Describe { get; set; }

      public string ImgUrl { get; set; }

      public string Introduce { get; set; }

   }
}
