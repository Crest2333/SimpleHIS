using System;
using System.Collections.Generic;
using System.Text;
using ABPExample.Domain.Public;

namespace HIS.Domain.Models
{
    public class ChatLog:FullEntity
    {

        public int DoctorId { get; set; }

        public int PatientUserId { get; set; }

        public string Message { get; set; }


        public bool? IsNew { get; set; }

        /// <summary>
        ///     1患者 2医生
        /// </summary>
        public int? SendFrom { get; set; }

    }
}
