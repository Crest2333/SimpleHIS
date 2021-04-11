using ABPExample.Application.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ABPExample.Domain.Dtos.Common;
using ABPExample.Domain.Dtos.Doctor;
using ABPExample.Domain.Dtos.MedicalAdvice;
using ABPExample.Domain.Dtos.Scheduling;
using ABPExample.Domain.Public;
using ABPExample.Query.Interface;
using HIS.Domain.Dtos.Doctor;
using Volo.Abp.DependencyInjection;

namespace ABPExample.Application.Application
{
    public class DoctorApplication:IDoctorApplication, ITransientDependency
    {
        private readonly IDoctorQuery _doctorQuery;
        private readonly IMedicalAdviceQuery _medicalAdviceQuery;

        public DoctorApplication(IDoctorQuery doctorQuery,IMedicalAdviceQuery medicalAdviceQuery)
        {
            _doctorQuery = doctorQuery;
            _medicalAdviceQuery = medicalAdviceQuery;
        }
        public async Task<ModelResult<List<SchedulingDto>>> GetSchedulingByUserId(int userId, DateTime? startDate, DateTime? endDate, int departmentId)
        {
            return await _doctorQuery.GetSchedulingByUserId(userId, startDate, endDate,departmentId);
        }

        public async Task<ModelResult> AddOrEditMedicalAdviceAsync(MedicalAdviceInputDto inputDto)
        {
            var entity = await GetMedicalAdviceAsync(inputDto.AppointmentId);

            if (entity.Result != null)
                return await _medicalAdviceQuery.EditMedicalAdviceAsync(inputDto);
            else return await _medicalAdviceQuery.AddMedicalAdviceAsync(inputDto);
        }

        public async Task<ModelResult<MedicalAdviceEntityDto>> GetMedicalAdviceAsync(int appointmentId)
        {
            return await _medicalAdviceQuery.GetMedicalAdviceAsync(appointmentId);
        }

        public async Task<ModelResult<PageDto<DoctorEntityDto>>> GetDoctorListAsync(GetDoctorInputDto param)
        {
           return await _doctorQuery.GetDoctorListAsync(param);
        }
    }
}
