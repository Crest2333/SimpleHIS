using ABPExample.Domain.Dtos.Common;
using ABPExample.Domain.Dtos.Patient;
using ABPExample.Domain.Public;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ABPExample.Domain.Dtos.Scheduling;

namespace ABPExample.Application.Interface
{
    public interface IDoctorApplication
    {
        Task<ModelResult<List<SchedulingDto>>> GetSchedulingByUserId(int userId, DateTime? startDate, DateTime? endDate, int departmentId);
    }
}
