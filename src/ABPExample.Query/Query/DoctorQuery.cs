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
using HIS.Domain.Dtos.Doctor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace ABPExample.Query.Query
{
   public class DoctorQuery : IDoctorQuery, ITransientDependency
   {
      private readonly IAppDbContext _context;

      public DoctorQuery(IAppDbContext context)
      {
         _context = context;
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

         var query = param.DepartmentId <= 0
            ? from a in _context.Users
              join b in _context.RoleMapper on a.Id equals b.UserId
              join c in _context.Doctor on a.Id equals c.UserId
              where !a.IsDeleted && !b.IsDeleted && !c.IsDeleted
              where param.Name.IsNull() || a.UserName == param.Name
              select new DoctorEntityDto
              {
                 Id = a.Id,
                 Name = a.UserName,
                 WorkNo = a.UserAccount,
                 Gender =
                    a.Gender == Domain.Models.Gender.Man ? "男" : a.Gender == Domain.Models.Gender.Woman ? "女" : "其他",
                 ImgUrl = c.DoctorImg,
                 Introduce = c.Introduce

              }
            : from a in _context.Users
              join b in _context.RoleMapper on a.Id equals b.UserId
              join c in _context.Doctor on a.Id equals c.UserId
              join d in _context.DepartmentMapper on a.Id equals d.UserId
              where !a.IsDeleted && !b.IsDeleted && !c.IsDeleted && !d.IsDeleted
              where param.Name.IsNull() || a.UserName == param.Name
              where param.DepartmentId == d.DepartmentId
              select new DoctorEntityDto
              {
                 Id = a.Id,
                 Name = a.UserName,
                 WorkNo = a.UserAccount,
                 Gender =
                    a.Gender == Domain.Models.Gender.Man ? "男" : a.Gender == Domain.Models.Gender.Woman ? "女" : "其他",
                 ImgUrl = c.DoctorImg,
                 Introduce=c.Introduce
              };

         var count = await query.CountAsync();
         var list = await query.ToListAsync(param.PageIndex, param.PageIndex);
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

      public async Task<ModelResult<PageDto<DoctorInfoDto>>> ListAsync(GetDoctorInputDto param)
      {
         var query = from a in _context.Users
                     from b in _context.DepartmentMapper.Where(c => c.UserId == a.Id)
                     from d in _context.Department.Where(c => c.Id == b.DepartmentId)
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
