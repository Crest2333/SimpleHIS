using ABPExample.Domain.Dtos.Common;
using ABPExample.Domain.Dtos.Patient;
using ABPExample.Domain.Public;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ABPExample.Domain.Dtos.Doctor;
using ABPExample.Domain.Dtos.MedicalAdvice;
using ABPExample.Domain.Dtos.Scheduling;
using HIS.Domain.Dtos.Doctor;

namespace ABPExample.Application.Interface
{
    public interface IDoctorApplication
    {
        Task<ModelResult<List<SchedulingDto>>> GetSchedulingByUserId(int userId, DateTime? startDate, DateTime? endDate, int departmentId);

        Task<ModelResult> AddOrEditMedicalAdviceAsync(MedicalAdviceInputDto inputDto);

        Task<ModelResult<MedicalAdviceEntityDto>> GetMedicalAdviceAsync(int appointmentId);

        Task<ModelResult<PageDto<DoctorEntityDto>>> GetDoctorListAsync(GetDoctorInputDto param);
    }
}
