using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ABPExample.Domain.Public;

namespace HIS.Domain.Models
{
   [Table("Doctors")]
   public class Doctor:FullEntity
   {
      public string Name  { get; set; }

      public string DoctorImg { get; set; }

      public string Introduce { get; set; }

      public int UserId { get; set; }

   }
}
