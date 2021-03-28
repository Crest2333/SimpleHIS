using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using  System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ABPExample.Domain.Dtos.MedicalAdvice;
using ABPExample.Domain.Models;
using ABPExample.Domain.Public;
using ABPExample.EntityFramework.EntityFrameworkCore;
using ABPExample.Query.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace ABPExample.Query.Query
{
    public class MedicalAdviceQuery:IMedicalAdviceQuery, ITransientDependency
    {
        private readonly IAppDbContext _context;
        private readonly IObjectMapper<ABPExampleQueryModule> _mapper;
        private readonly IUserQuery _userQuery;

        public MedicalAdviceQuery(IAppDbContext context,IObjectMapper<ABPExampleQueryModule> mapper,IUserQuery userQuery)
        {
            _context = context;
            _mapper = mapper;
            _userQuery = userQuery;
        }

        public async Task<ModelResult<MedicalAdviceEntityDto>> GetMedicalAdviceAsync(int appointmentId)
        {
            var entity = await (
                from a in _context.MedicalAdvice
                join b in _context.Users on a.DoctorId equals b.Id
                join c in _context.Patients on a.PatientId equals c.Id
                where a.AppointmentId == appointmentId && !a.IsDeleted
                select new MedicalAdviceEntityDto
                {
                    Content = a.Content,
                    PatientName = c.FullName,
                    DoctorId = b.Id,
                    DoctorName = b.UserName,
                    DoctorNo = b.UserAccount,
                    CreateDate = b.CreationTime,
                    EditDate = b.LastModificationTime,
                }).FirstOrDefaultAsync();

            return  new ModelResult<MedicalAdviceEntityDto> {Result = entity};
        }

        public async Task<ModelResult> AddMedicalAdviceAsync(MedicalAdviceInputDto input)
        {
            var appointment = await _context.Appointment.AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == input.AppointmentId);
            if (appointment == null)
                return new ModelResult {IsSuccess = false, Message = "无效Id"};

            var userInfo = await _userQuery.GetUserInfoDetail(appointment.DoctorId);
            var entity = new MedicalAdvice
            {
                AppointmentId = appointment.Id,
                DoctorId = appointment.DoctorId,
                DoctorNo = userInfo.UserAccount,
                PatientId = appointment.PatientId,
                Content = input.Content,

            };
            await _context.AddAsync(entity);

            await _context.SaveChangesAsync();
            return new ModelResult { IsSuccess = true, Message = "添加成功" };
        }

        public async Task<ModelResult> EditMedicalAdviceAsync(MedicalAdviceInputDto input)
        {
            var entity =
                await _context.MedicalAdvice.FirstOrDefaultAsync(c =>
                    c.AppointmentId == input.AppointmentId && !c.IsDeleted);
            if (entity == null)
                return new ModelResult {IsSuccess = false, Message = "无效Id"};

            entity.Content = input.Content;
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return new ModelResult {IsSuccess = true, Message = "编辑成功"};

        }
    }
}
