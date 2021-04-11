using ABPExample.Domain.Dtos.Patient;
using ABPExample.Domain.Public;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ABPExample.Domain.Dtos.Common;
using ABPExample.Domain.Dtos.MedicalHistory;

namespace ABPExample.Application.Interface
{
    public interface IPatientApplication
    {

        Task<ModelResult<int>> Add(AddPatientInfoDto model);

        Task<ModelResult> Delete(int id);

        Task<ModelResult> Edit(EditPatientInfoDto model);

        Task<ModelResult<PageDto<PatientInfoListDto>>> List(PatientSearchDto param);

        Task<ModelResult> AddIllnessHistory(AddPastHistoryDto model);

        Task<ModelResult<PatientInfoDetailDto>> Detail(int id);

        Task<ModelResult> BatchDelete(List<long> id);

        Task<ModelResult> BatchAdd(List<AddPatientInfoDto> modelList);

        Task<ModelResult> BatchAddIllnessHistory(List<AddPastHistoryDto> modelList);

        Task<PatientInfoDetailDto> GetPatientByUserIdAsync(int userId);

   }
}
