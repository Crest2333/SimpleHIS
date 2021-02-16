using ABPExample.Application.Interface;
using ABPExample.Domain.Dtos.Common;
using ABPExample.Domain.Dtos.Patient;
using ABPExample.Domain.Public;
using ABPExample.Query.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ABPExample.Domain.Dtos.MedicalHistory;
using Volo.Abp.DependencyInjection;

namespace ABPExample.Application.Application
{
    public class PatientApplication : IPatientApplication, ITransientDependency
    {
        private readonly IPatientQuery _patientQuery;

        public PatientApplication(IPatientQuery patientQuery)
        {
            _patientQuery = patientQuery;
        }

        public async Task<ModelResult> Add(AddPatientInfoDto model)
        {
            return await _patientQuery.Add(model);
        }

        public async Task<ModelResult> AddIllnessHistory(AddPastHistoryDto model)
        {
            return await _patientQuery.AddIllnessHistory(model);
        }

        public async Task<ModelResult> BatchAdd(List<AddPatientInfoDto> modelList)
        {
            return await _patientQuery.BatchAdd(modelList);
        }

        public async Task<ModelResult> BatchAddIllnessHistory(List<AddPastHistoryDto> modelList)
        {
            return await _patientQuery.BatchAddIllnessHistory(modelList);
        }

        public async Task<ModelResult> BatchDelete(List<long> id)
        {
            return await _patientQuery.BatchDelete(id);
        }

        public async Task<ModelResult> Delete(int id)
        {
            return await _patientQuery.Delete(id);
        }

        public async Task<ModelResult<PatientInfoDetailDto>> Detail(int id)
        {
            return await _patientQuery.Detail(id);
        }

        public async Task<ModelResult> Edit(EditPatientInfoDto model)
        {
            return await _patientQuery.Edit(model);
        }

        public async Task<ModelResult<PageDto<PatientInfoListDto>>> List(PatientSearchDto param)
        {
            return await _patientQuery.List(param);
        }
    }
}
