using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABPExample.Domain.Dtos.Appointment;
using ABPExample.Domain.Dtos.Common;
using ABPExample.Domain.Models;
using ABPExample.Domain.Models.Enum;
using ABPExample.Domain.Public;
using ABPExample.EntityFramework.EntityFrameworkCore;
using ABPExample.Query.Interface;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;

namespace ABPExample.Query.Query
{
    public class PAQuery : IPAQuery, ITransientDependency
    {
        private readonly IAppDbContext _context;

        public PAQuery(IAppDbContext context)
        {
            _context = context;

        }
        public async Task<ModelResult> AddAppointment(AddAppointmentInfoDto inputDto)
        {
            var patientInfo = await _context.Patients.FirstOrDefaultAsync(c => c.Id == inputDto.PatientId && !c.IsDeleted);
            if (patientInfo == null)
                return new ModelResult { IsSuccess = false, Message = "没有找到患者信息" };

            var query = new Appointment
            {
                Status = AppointmentStatusEnum.Reserved,
                AppointmentDate = inputDto.AppointmentDate,
                CreationTime = DateTime.Now,
                DepartmentId = inputDto.DepartmentId,
                Describe = inputDto.Describe,
                DoctorId = inputDto.DoctorId,
                AppointmentNo = "10",
                AppointmentTime = $"{Convert.ToDateTime(inputDto.AppointmentTime):HH:mm}",
                IsDeleted = false,
                PatientId = inputDto.PatientId
            };

            await _context.AddAsync(query);
            await _context.SaveChangesAsync();
            return new ModelResult { IsSuccess = true, Message = "添加成功" };
        }

        public Task<ModelResult> BatchAddAppointment(List<AddAppointmentInfoDto> infoDtoList)
        {
            throw new NotFiniteNumberException();

        }

        public async Task<ModelResult> DeleteAppointment(int id)
        {
            var query = await _context.Appointment.FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
            if (query == null)
                return new ModelResult { IsSuccess = false, Message = "没有找到相关预约信息" };
            query.IsDeleted = true;
            query.LastModificationTime = DateTime.Now;

            _context.Update(query);
            await _context.SaveChangesAsync();
            return new ModelResult { IsSuccess = true, Message = "删除成功" };
        }

        public async Task<ModelResult<List<AppointmentInfoListDto>>> GetAppointmentByDoctor(int doctorId, DateTime? strartDate, DateTime? endDate)
        {
            throw new NotImplementedException();
        }

        public async Task<ModelResult<AppointmentInfoDetailDto>> GetAppointmentInfoDetail(int id)
        {
            var result = await (
                from a in _context.Appointment
                join b in _context.Patients on a.PatientId equals b.Id
                join c in _context.Users on a.DoctorId equals c.Id
                join d in _context.Department on a.DepartmentId equals d.Id
                where a.Id == id && !a.IsDeleted
                select new AppointmentInfoDetailDto
                {
                    Id = a.Id,
                    Status = a.Status,
                    AppointmentDate = a.AppointmentDate,
                    Department = d.Name,
                    Describe = a.Describe,
                    DoctorName = c.UserName,
                    DoctorNo = c.UserAccount,
                    Gender = b.Gender.ToString(),
                    PatientName = b.FullName,
                    Address = b.Address,
                    BloodType = b.BloodType,
                    DateOfBirth = b.DateOfBirth,
                    Height = b.Height,
                    IdentityId = b.IdentityId,
                    PhoneNumber = b.PhoneNumber,
                    Weight = b.Weight
                }).FirstOrDefaultAsync();
            return new ModelResult<AppointmentInfoDetailDto> { IsSuccess = true, Result = result };
        }

        public async Task<ModelResult<PageDto<AppointmentInfoListDto>>> GetAppointmentInfoList(AppointmentInfoListSearchDto param)
        {
            var query = from a in _context.Appointment
                        join b in _context.Patients on a.PatientId equals b.Id
                        join c in _context.Users on a.DoctorId equals c.Id
                        join d in _context.Department on a.DepartmentId equals d.Id
                        where !param.PatientId.HasValue || a.PatientId == param.PatientId
                        where string.IsNullOrWhiteSpace(param.DoctorName) || c.UserName == param.DoctorName
                        where string.IsNullOrWhiteSpace(param.PatientName) || b.FullName == param.PatientName
                        where string.IsNullOrWhiteSpace(param.PhoneNumber) || b.PhoneNumber == param.PhoneNumber
                        where !param.Status.HasValue ||(int) a.Status == param.Status
                        where !param.StartDate.HasValue || a.AppointmentDate >= param.StartDate.Value
                        where !param.EndDate.HasValue || a.AppointmentDate <= param.EndDate
                        where !a.IsDeleted
                        select new AppointmentInfoListDto
                        {
                            IdentityId = b.IdentityId,
                            PhoneNumber = b.PhoneNumber,
                            Status = a.Status,
                            AppointmentDate = a.AppointmentDate,
                            Department = d.Name,
                            DoctorName = c.UserName,
                            DoctorNo = c.UserAccount,
                            Gender = b.Gender,
                            Id = a.Id,
                            PatientName = b.FullName,
                            AppointmentNo = a.AppointmentNo
                        };


            var count = await query.CountAsync();

            var list = await query.Skip((param.PageIndex - 1) * param.PageSize)
                .Take(param.PageSize).ToListAsync();
            return new ModelResult<PageDto<AppointmentInfoListDto>> { IsSuccess = true, Result = new PageDto<AppointmentInfoListDto>(count, list) };
        }

        public async Task<ModelResult<bool>> ChangeAppointmentStatus(EditAppointmentInputDto inputDto)
        {
            var appointment =
                await _context.Appointment
                    .Where(c => c.Id == inputDto.AppointmentId&&!c.IsDeleted)
                    .FirstOrDefaultAsync();

            if (appointment == null)
                return new ModelResult<bool> {IsSuccess = false, Message = "无效Id"};
            appointment.Status = inputDto.Status;
            _context.Update(appointment);

            return new ModelResult<bool> {Result = await _context.SaveChangesAsync() > 0};

        }
    }
}
