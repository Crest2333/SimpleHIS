using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ABPExample.EntityFramework.EntityFrameworkCore;
using ABPExample.Query;
using ABPExample.Query.Common;
using HIS.Domain.Dtos.Chat;
using HIS.Domain.Models;
using HIS.Query.Interface;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace HIS.Query.Query
{
    public class ChatQuery : IChatQuery, ITransientDependency
    {
        private readonly IAppDbContext _context;
        private readonly IObjectMapper<HISQueryModule> _mapper;

        public ChatQuery(IAppDbContext context, IObjectMapper<HISQueryModule> mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ChatLogDto>> GetChatLogByDoctorIdAsync(int doctorId, int patientUserId, DateTime? startDateTime)
        {
            var list = await _context.ChatLog
                .Where(c => c.DoctorId == doctorId && c.PatientUserId == patientUserId && !c.IsDeleted)
                .WhereIf(startDateTime.HasValue, c => c.CreationTime >= startDateTime.Value)
                .ToListAsync();
            return _mapper.Map<List<ChatLog>, List<ChatLogDto>>(list);
        }

        public async Task<bool> AddAsync(string message, int patientUserId, int doctorId, int from)
        {
            var model = new ChatLog
            {
                PatientUserId = patientUserId,
                DoctorId = doctorId,
                Message = message,
                SendFrom = from,
            };
            await _context.AddAsync(model);
            return await _context.SaveChangesAsync()>0;
        }
    }
}
