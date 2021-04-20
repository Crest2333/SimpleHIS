using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABPExample.EntityFramework.EntityFrameworkCore;
using ABPExample.Query;
using AutoMapper;
using HIS.Domain.Dtos.PatientUser;
using HIS.Domain.Models;
using HIS.Query.Interface;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace HIS.Query.Query
{
   public  class PatientUserQuery:IPatientUserQuery,ITransientDependency
    {
        private readonly IAppDbContext _context;
        private readonly IObjectMapper<HISQueryModule> _mapper;

        public PatientUserQuery(IAppDbContext context,IObjectMapper<HISQueryModule> mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Dictionary<int, string>> AuthenticationAsync(string account, string passWord)
        {
            return await _context.PatientUser
                .Where(c => (c.Email == account || c.PhoneNumber == account) && c.UserPwd == passWord)
                .ToDictionaryAsync(c => c.Id, c => c.UserName);
        }

        public async Task<bool> RegisterAsync(RegisterInputDto input)
        {
            var entity = _mapper.Map<RegisterInputDto, PatientUser>(input);
            entity.Init();
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditPatientUserAsync(PatientInputDto input)
        {
            var entity = _mapper.Map<PatientInputDto, PatientUser>(input);
            _context.Modified(entity,c=>c.PhoneNumber);
            _context.Modified(entity, c => c.Email);
            _context.Modified(entity, c => c.UserImg);
            _context.Modified(entity, c => c.UserPwd);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddPatientMapperAsync(int userId, int patientId)
        {
           var entity = new PatientsMapping {UserId = userId, PatientId = patientId};
           await _context.AddAsync(entity);
           return await _context.SaveChangesAsync() > 0;
        }
    }
}
