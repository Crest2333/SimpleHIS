using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABPExample.Domain.Models;
using ABPExample.Domain.Models.Enum;
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
                .Where(c =>c.AppointmentDate <=
                    DateTime.Now.AddHours(-1) && c.Status == AppointmentStatusEnum.Reserved)
                .ToListAsync();

            return list;
        }
    }
}
