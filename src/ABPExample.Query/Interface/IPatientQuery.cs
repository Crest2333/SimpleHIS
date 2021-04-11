using ABPExample.Domain.Dtos.Patient;
using ABPExample.Domain.Public;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ABPExample.Domain.Dtos.Common;
using ABPExample.Domain.Dtos.MedicalHistory;

namespace ABPExample.Query.Interface
{
    public interface IPatientQuery
    {
        Task<ModelResult<int>> Add(AddPatientInfoDto model);

        Task<ModelResult> Delete(int id);

        Task<ModelResult> Edit(EditPatientInfoDto model);

        Task<ModelResult<PageDto<PatientInfoListDto>>> List(PatientSearchDto param);

        Task<ModelResult> AddIllnessHistory(AddPastHistoryDto model);

        /// <summary>
        ///    获取根据身份证号获取患者信息
        /// </summary>
        /// <param name="identityId"></param>
        /// <returns></returns>
        Task<PatientInfoDetailDto> GetPatientDetailAsync(string identityId);

        Task<ModelResult<PatientInfoDetailDto>> Detail(int id);

        Task<ModelResult> BatchDelete(List<long> id);

        Task<ModelResult> BatchAdd(List<AddPatientInfoDto> modelList);

        Task<ModelResult> BatchAddIllnessHistory(List<AddPastHistoryDto> modelList);

        /// <summary>
        ///    获取患者信息
        /// </summary>
        /// <param name="userId">患者账号Id</param>
        /// <returns></returns>
        Task<PatientInfoDetailDto> GetPatientByUserIdAsync(int userId);
    }
}
