using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HIS.Domain.Dtos.Chat;

namespace HIS.Query.Interface
{
    public interface IChatQuery
    {
        Task<List<ChatLogDto>> GetChatLogByDoctorIdAsync(int doctorId, int patientUserId, DateTime? startDateTime);

        Task<bool> AddAsync(string message,int patientUserId,int doctorId,int from);
    }
}
