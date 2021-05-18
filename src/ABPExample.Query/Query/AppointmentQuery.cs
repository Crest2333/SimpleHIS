using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABPExample.Domain.Dtos.Appointment;
using ABPExample.Domain.Models;
using ABPExample.Domain.Models.Enum;
using ABPExample.Domain.Public;
using ABPExample.EntityFramework.EntityFrameworkCore;
using ABPExample.Query.Interface;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;

namespace HIS.Query.Query
{
    public class AppointmentQuery : IAppointmentQuery, ITransientDependency
    {
        private readonly IAppDbContext _context;

        public AppointmentQuery(IAppDbContext context)
        {
            _context = context;
        }

        public async Task BatchUpdate(List<Appointment> entityList)
        {
            _context.UpdateRange(entityList);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Appointment>> GetExpireAppointmentAsync()
        {
            var list = await _context.Appointment
                .Where(c => c.AppointmentDate <=
                    DateTime.Now.AddHours(-1) && c.Status == AppointmentStatusEnum.Reserved)
                .ToListAsync();

            return list;
        }

        public async Task<ModelResult> EditAppointmentAsync(EditAppointmentInputDto inputDto)
        {
            var appointment =
                await _context.Appointment.FirstOrDefaultAsync(c => c.Id == inputDto.AppointmentId && !c.IsDeleted);
            if (appointment == null)
                return new ModelResult { IsSuccess = false, Message = "无效Id" };
            var time = $"{Convert.ToDateTime(inputDto.AppointmentTime):HH:mm}";
            var haveEntity = await _context.Appointment
                .AnyAsync(c =>
                    c.PatientId == appointment.PatientId && c.AppointmentDate.Date == inputDto.AppointmentDate.Date &&
                    c.AppointmentTime == time && !c.IsDeleted && c.Status == AppointmentStatusEnum.Reserved && c.Id != inputDto.AppointmentId);
            if (haveEntity)
                return new ModelResult { IsSuccess = false, Message = "所修改时间段已有别的预约正在进行" };

            appointment.AppointmentDate = Convert.ToDateTime($"{inputDto.AppointmentDate:yyyy-MM-dd} {time}");
            appointment.DepartmentId = inputDto.DepartmentId;
            appointment.DoctorId = inputDto.DoctorId;
            appointment.Describe = inputDto.Describe;
            appointment.AppointmentTime = time;

            if (appointment.AppointmentDate < DateTime.Now)
                return new ModelResult { IsSuccess = false, Message = "预约时间不能小于当前时间" };

            _context.Update(appointment);
            return new ModelResult { IsSuccess = await _context.SaveChangesAsync() > 0 };
        }
    }
}
