using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ABPExample.Application.Interface;
using ABPExample.Domain.Dtos.Common;
using ABPExample.Domain.Dtos.MedicalHistory;
using ABPExample.Domain.Public;
using ABPExample.Query.Interface;
using Volo.Abp.DependencyInjection;

namespace ABPExample.Application.Application
{
    public class MedicalHistoryApplication : IMedicalHistoryApplication, ITransientDependency
    {
        private readonly IMedicalHistoryQuery _medicalHistoryQuery;

        public MedicalHistoryApplication(IMedicalHistoryQuery medicalHistoryQuery)
        {
            _medicalHistoryQuery = medicalHistoryQuery;
        }
        public async Task<ModelResult<PageDto<MedicalInfoDto>>> List(GetMedicalInputDto param)
        {
            return await _medicalHistoryQuery.List(param);

        }

        public async Task<ModelResult> Add(AddPastHistoryDto input)
        {
            return await _medicalHistoryQuery.Add(input);
        }

        public async Task<ModelResult> Delete(int id)
        {
            return await _medicalHistoryQuery.Delete(id);
        }

        public async Task<ModelResult> Edit(EditMedicalInputDto input)
        {
            return await _medicalHistoryQuery.Edit(input);
        }

        public async Task<ModelResult<MedicalInfoDto>> Detail(int id)
        {
            return await _medicalHistoryQuery.Detail(id);
        }
    }
}
