using ABPExample.Application.Interface;
using ABPExample.Domain.Dtos.Common;
using ABPExample.Domain.Dtos.Patient;
using ABPExample.Domain.Public;
using ABPExample.Query.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
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

        public async Task<ModelResult<int>> Add(AddPatientInfoDto model)
        {
            if (model.FullName.IsNullOrWhiteSpace())
                return new ModelResult<int> { IsSuccess = false, Message = "请输入名称" };
            if (model.PhoneNumber.IsNullOrWhiteSpace())
                return new ModelResult<int> { IsSuccess = false, Message = "请输入电话号码" };

            if (model.IdentityId.IsNullOrWhiteSpace())
                return new ModelResult<int> { IsSuccess = false, Message = "请输入身份证号" };
            if (model.IdentityId.Length != 18)
                return new ModelResult<int> { IsSuccess = false, Message = "身份证号格式错误" };
            var rexPhone = new Regex(@"^\d+$");
            if (!rexPhone.IsMatch(model.PhoneNumber))
                return new ModelResult<int> { IsSuccess = false, Message = "电话必须为数字" };

            return await _patientQuery.Add(model);
        }

        public async Task<ModelResult> AddIllnessHistory(AddPastHistoryDto model)
        {
            if (model.Name.IsNullOrWhiteSpace())
            {
                return new ModelResult { IsSuccess = false, Message = "请填写名称" };
            }

            if (model.Describe.IsNullOrWhiteSpace())
            {
                return new ModelResult { IsSuccess = false, Message = "请填写描述" };
            }
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

        public async Task<PatientInfoDetailDto> GetPatientByUserIdAsync(int userId)
        {
            return await _patientQuery.GetPatientByUserIdAsync(userId);
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
            if (model.FullName.IsNullOrWhiteSpace())
                return new ModelResult { IsSuccess = false, Message = "请输入名称" };
            if (model.PhoneNumber.IsNullOrWhiteSpace())
                return new ModelResult { IsSuccess = false, Message = "请输入电话号码" };

            if (model.IdentityId.IsNullOrWhiteSpace())
                return new ModelResult{ IsSuccess = false, Message = "请输入身份证号" };
            if (model.IdentityId.Length != 18)
                return new ModelResult{ IsSuccess = false, Message = "身份证号格式错误" };
            var rexPhone = new Regex(@"^\d+$");
            if (!rexPhone.IsMatch(model.PhoneNumber))
                return new ModelResult { IsSuccess = false, Message = "电话必须为数字" };
            return await _patientQuery.Edit(model);
        }

        public async Task<ModelResult<PageDto<PatientInfoListDto>>> List(PatientSearchDto param)
        {
            return await _patientQuery.List(param);
        }
    }
}
