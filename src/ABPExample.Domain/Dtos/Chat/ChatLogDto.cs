using System;
using System.Collections.Generic;
using System.Text;

namespace HIS.Domain.Dtos.Chat
{
    public class ChatLogDto
    {
        public int DoctorId { get; set; }

        public int PatientUserId { get; set; }

        public string Message { get; set; }

        public int? SendFrom { get; set; }

        public bool IsMe { get; set; }

        public DateTime CreationTime { get; set; }

        public string SendDateTime => $"{CreationTime:yyyy-MM-dd HH:mm:ss}";
    }
}
