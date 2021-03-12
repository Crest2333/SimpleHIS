using ABPExample.Application.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ABPExample.Domain.Dtos.Scheduling;
using ABPExample.Domain.Public;
using ABPExample.Query.Interface;
using Volo.Abp.DependencyInjection;

namespace ABPExample.Application.Application
{
    public class DoctorApplication:IDoctorApplication, ITransientDependency
    {
        private readonly IDoctorQuery _doctorQuery;

        public DoctorApplication(IDoctorQuery doctorQuery)
        {
            _doctorQuery = doctorQuery;
        }
        public async Task<ModelResult<List<SchedulingDto>>> GetSchedulingByUserId(int userId, DateTime? startDate, DateTime? endDate, int departmentId)
        {
            return await _doctorQuery.GetSchedulingByUserId(userId, startDate, endDate,departmentId);
        }
    }
}
