using ABPExample.Domain.Dtos.PastHistory;
using ABPExample.Domain.Dtos.Patient;
using ABPExample.Domain.Public;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ABPExample.Query.Interface
{
    public interface IPatientQuery
    {
        Task<ModelResult> Add(AddPatientInfoDto model);

        Task<ModelResult> Delete(int id);

        Task<ModelResult> Edit(EditPatientInfoDto model);

        Task<ModelResult<List<PatientInfoListDto>>> List(PatientSearchDto param);

        Task<ModelResult> AddIllnessHistory(AddPastHistoryDto model);

        Task<ModelResult<PatientInfoDetailDto>> Detail(int id);

        Task<ModelResult> BatchDelete(List<long> id);

        Task<ModelResult> BatchAdd(List<AddPatientInfoDto> modelList);

        Task<ModelResult> BatchAddIllnessHistory(List<AddPastHistoryDto> modelList);
    }
}
