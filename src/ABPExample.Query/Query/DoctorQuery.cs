using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ABPExample.Domain.Dtos.Common;
using ABPExample.Domain.Dtos.Doctor;
using ABPExample.Domain.Public;
using ABPExample.EntityFramework.EntityFrameworkCore;
using ABPExample.Query.Interface;
using Volo.Abp.DependencyInjection;
using System.Linq;
using System.Threading.Tasks.Dataflow;
using ABPExample.Domain.Dtos.Scheduling;
using ABPExample.Query.Common;
using HIS.Domain.Dtos.Chat;
using HIS.Domain.Dtos.Doctor;
using HIS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Volo.Abp.ObjectMapping;

namespace ABPExample.Query.Query
{
    public class DoctorQuery : IDoctorQuery, ITransientDependency
    {
        private readonly IAppDbContext _context;
        private readonly IObjectMapper<HISQueryModule> _mapper;

        public DoctorQuery(IAppDbContext context, IObjectMapper<HISQueryModule> mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ModelResult<DoctorInfoDto>> DetailAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ModelResult<List<SchedulingDto>>> GetSchedulingByUserId(int userId, DateTime? startDate, DateTime? endDate, int departmentId)
        {
            var scheduling = await _context.Scheduling.Where(c => c.UserId == userId && !c.IsDeleted)
                .Where(c => c.StartDate <= endDate.Value && c.EndDate >= startDate.Value)
                .Where(c => c.DepartmentId == departmentId)
                .ToListAsync();

            if (!scheduling.Any())
                return new ModelResult<List<SchedulingDto>> { IsSuccess = false, Message = "该名医生在该时间段没有排班" };
            var appointmentList = await _context.Appointment.Where(c => c.DoctorId == userId && !c.IsDeleted)
                .Where(c => (int)c.Status != -1)
                .Where(c => c.AppointmentDate >= startDate && c.AppointmentDate <= endDate)
                .ToListAsync();
            var result = new List<SchedulingDto>();
            while (startDate.Value.Date <= endDate.Value.Date)
            {
                var item = new SchedulingDto
                {
                    SchedulingDate = $"{startDate.Value:yyyy-MM-dd}",
                };
                foreach (var time in ConstValue.AppointmentTime)
                {
                    if (appointmentList.All(c => c.AppointmentTime != time))
                    {
                        item.AppointmentNum.Add(time);
                    }
                }
                result.Add(item);
                startDate = startDate.Value.AddDays(1);
            }

            return new ModelResult<List<SchedulingDto>> { Result = result };
        }

        public async Task<ModelResult<PageDto<DoctorEntityDto>>> GetDoctorListAsync(GetDoctorInputDto param)
        {

            var query = GetDoctorQuery(param);

            var count = await query.CountAsync();
            var list = await query.ToListAsync(param.PageIndex, param.PageSize);
            var userIdList = list.Select(c => c.Id).Distinct().ToList();
            var departmentInfoList = await (from a in _context.DepartmentMapper
                                            join b in _context.Department on a.DepartmentId equals b.Id
                                            where userIdList.Contains(a.UserId) && !a.IsDeleted
                                            select new
                                            {
                                                a.UserId,
                                                b.Id,
                                                b.Name
                                            }).ToListAsync();
            foreach (var item in list)
            {
                item.DepartmentList = departmentInfoList.Where(c => c.UserId == item.Id).Select(c => c.Name).ToList();
            }


            return new ModelResult<PageDto<DoctorEntityDto>>
            { Result = new PageDto<DoctorEntityDto>(count, list) };
        }

        private IQueryable<DoctorEntityDto> GetDoctorQuery(GetDoctorInputDto param)
        {
            if (param.IsManager.HasValue)
            {
                return from a in _context.Users
                       join b in _context.RoleMapper on a.Id equals b.UserId
                       from c in _context.Doctor.Where(e => e.UserId == a.Id).DefaultIfEmpty()
                       where !a.IsDeleted && !b.IsDeleted && (c == null || !c.IsDeleted)
                       where param.Name.IsNull() || a.UserName == param.Name
                       where b.RoleId == 1
                       select new DoctorEntityDto
                       {
                           Id = a.Id,
                           Name = a.UserName,
                           WorkNo = a.UserAccount,
                           Gender =
                               a.Gender == Domain.Models.Gender.Man ? "男" :
                               a.Gender == Domain.Models.Gender.Woman ? "女" : "其他",
                           ImgUrl = c.DoctorImg,
                           Introduce = c.Introduce,
                           LastModificationTime = c.LastModificationTime
                       };
            }
            else
            {
                return param.DepartmentId <= 0
                     ? from a in _context.Users
                       join b in _context.RoleMapper on a.Id equals b.UserId
                       join c in _context.Doctor on a.Id equals c.UserId
                       where !a.IsDeleted && !b.IsDeleted && !c.IsDeleted
                       where param.Name.IsNull() || a.UserName == param.Name
                       where param.SearchValue.IsNull() || a.UserName.Contains(param.SearchValue)
                       where b.RoleId == 1
                       select new DoctorEntityDto
                       {
                           Id = a.Id,
                           Name = a.UserName,
                           WorkNo = a.UserAccount,
                           Gender =
                                a.Gender == Domain.Models.Gender.Man ? "男" : a.Gender == Domain.Models.Gender.Woman ? "女" : "其他",
                           ImgUrl = c.DoctorImg,
                           Introduce = c.Introduce,
                           LastModificationTime = c.LastModificationTime
                       }
                     : from a in _context.Users
                       join b in _context.RoleMapper on a.Id equals b.UserId
                       join c in _context.Doctor on a.Id equals c.UserId
                       join d in _context.DepartmentMapper on a.Id equals d.UserId
                       join f in _context.Department on d.DepartmentId equals f.Id
                       where !a.IsDeleted && !b.IsDeleted && !c.IsDeleted && !d.IsDeleted
                       where param.Name.IsNull() || a.UserName == param.Name
                       where param.SearchValue.IsNull() || a.UserName.Contains(param.SearchValue) || f.Name.Contains(param.SearchValue)
                       where param.DepartmentId == d.DepartmentId
                       where b.RoleId == 1
                       select new DoctorEntityDto
                       {
                           Id = a.Id,
                           Name = a.UserName,
                           WorkNo = a.UserAccount,
                           Gender =
                                a.Gender == Domain.Models.Gender.Man ? "男" : a.Gender == Domain.Models.Gender.Woman ? "女" : "其他",
                           ImgUrl = c.DoctorImg,
                           Introduce = c.Introduce,
                           LastModificationTime = c.LastModificationTime
                       };
            }
        }


        public async Task<ModelResult> EditDoctorInfoAsync(DoctorInfoInputDto inputDto)
        {
            var entity = await _context.Doctor.FirstOrDefaultAsync(c => c.UserId == inputDto.UserId && !c.IsDeleted);
            if (entity == null)
            {
                var model = _mapper.Map<DoctorInfoInputDto, Doctor>(inputDto);
                await _context.AddAsync(model);
                await _context.SaveChangesAsync();
                return new ModelResult();
            }
            else
            {
                _mapper.Map(inputDto, entity);
                _context.Update(entity);
                await _context.SaveChangesAsync();
                return new ModelResult();
            }
        }

        public async Task<DoctorEntityDto> GetDoctorInfoDetailAsync(int userId)
        {
            return await (from a in _context.Users
                          join b in _context.RoleMapper on a.Id equals b.UserId
                          from c in _context.Doctor.Where(e => e.UserId == a.Id).DefaultIfEmpty()
                          where !a.IsDeleted && !b.IsDeleted && (c == null || !c.IsDeleted)
                          where b.RoleId == 1
                          where a.Id == userId
                          select new DoctorEntityDto
                          {
                              Id = a.Id,
                              Name = a.UserName,
                              WorkNo = a.UserAccount,
                              Gender =
                                  a.Gender == Domain.Models.Gender.Man ? "男" :
                                  a.Gender == Domain.Models.Gender.Woman ? "女" : "其他",
                              ImgUrl = c.DoctorImg,
                              Introduce = c.Introduce,
                              LastModificationTime = c.LastModificationTime
                          }).FirstOrDefaultAsync();


        }

        public async Task<ModelResult<List<ChatUserInfoDto>>> GetOnlineUserAsync(int doctorId)
        {
            var list = await (
                from a in _context.ChatLog
                join b in _context.PatientUser on a.PatientUserId equals b.Id
                where !b.IsDeleted
                orderby a.CreationTime descending
                group new { a, b } by new { b.Id, b.UserName }
                into c
                select new ChatUserInfoDto
                {
                    UserId = c.Key.Id,
                    Name = c.Key.UserName
                }).ToListAsync();

            var userIdList = list.Select(c => c.UserId).ToList();

            var patientInfoList = await (
                from a in _context.PatientsMapping
                from b in _context.Patients.Where(c => c.Id == a.PatientId && !c.IsDeleted).DefaultIfEmpty()
                where !a.IsDeleted && userIdList.Contains(a.UserId)
                select new
                {
                    a.UserId,
                    b.FullName
                }).ToDictionaryAsync(c => c.UserId, c => c.FullName);


            var newMessage = await _context.ChatLog.Where(c =>
                    userIdList.Contains(c.PatientUserId) && c.DoctorId == doctorId && c.SendFrom == 1 &&
                    c.IsNew.HasValue && c.IsNew.Value && !c.IsDeleted)
                .GroupBy(c => c.PatientUserId)
                .Select(c => new {c.Key, TotalCount = c.Count()})
                .ToListAsync();

            foreach (var item in list)
            {
                if (patientInfoList.ContainsKey(item.UserId))
                    item.Name = patientInfoList[item.UserId];
                if (newMessage.Any(c => c.Key == item.UserId))
                {
                    item.NewMessage = newMessage.FirstOrDefault(c => c.Key == item.UserId)?.TotalCount;
                }
            }
            return ModelResult<List<ChatUserInfoDto>>.Instance.Ok("", list);
        }

        public async Task<ModelResult<PageDto<DoctorInfoDto>>> ListAsync(GetDoctorInputDto param)
        {
            var query = from a in _context.Users
                        from b in _context.DepartmentMapper.Where(c => c.UserId == a.Id)
                        from d in _context.Department.Where(c => c.Id == b.DepartmentId)
                        where !a.IsDeleted && !b.IsDeleted && !d.IsDeleted
                        where string.IsNullOrEmpty(param.Name) || a.UserName == param.Name
                        where param.DepartmentId <= 0 || param.DepartmentId == d.Id
                        orderby a.Id descending
                        select new DoctorInfoDto
                        {
                            WorkStartDate = b.CreationTime,
                            DepartmentId = d.Id,
                            DepartmentName = d.Name,
                            Gender = a.Gender == Domain.Models.Gender.Man ? "男" : a.Gender == Domain.Models.Gender.Woman ? "女" : "其他",
                            Id = a.Id,
                            Name = a.UserName,
                            WorkNo = a.UserAccount
                        };
            var count = await query.CountAsync();
            var list = await query.Skip((param.PageIndex - 1) * param.PageSize).Take(param.PageSize).ToListAsync();
            return new ModelResult<PageDto<DoctorInfoDto>> { IsSuccess = true, Result = new PageDto<DoctorInfoDto>(count, list) };
        }
    }
}
