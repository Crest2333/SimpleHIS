using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ABPExample.Domain.Dtos.Patient;
using ABPExample.Domain.Dtos.UserDtos;
using ABPExample.Domain.Public;
using HIS.Domain.Dtos.Chat;
using HIS.Domain.Dtos.PatientUser;
using RegisterInputDto = HIS.Domain.Dtos.PatientUser.RegisterInputDto;

namespace HIS.Application.Interface
{
    public interface IPatientUserApplication
    {
        Task<ModelResult> RegisterAsync(RegisterInputDto input);

        Task<ModelResult> LoginAsync(LoginInputDto input);

        /// <summary>
        ///   添加对应关系
        /// </summary>
        /// <param name="inputDto"></param>
        /// <param name="toInt"></param>
        /// <returns></returns>
        Task<ModelResult> AddPatientMapperAsync(AddPatientInfoDto inputDto, int toInt);

        Task<ModelResult<List<ChatLogDto>>> GetChatLogByDoctorIdAsync(int doctorId, int patientUserId, DateTime? startDateTime);

        Task<ModelResult<bool>> AddAsync(string message, int patientUserId, int doctorId,int from);

        Task<ModelResult> ResetPassWordAsync(ResetPassWordDto inputDto);
    }
}
