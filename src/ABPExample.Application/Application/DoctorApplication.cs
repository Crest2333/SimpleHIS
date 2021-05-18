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
using HIS.Application.Interface;
using HIS.Domain.Dtos.Chat;
using HIS.Domain.Dtos.Doctor;
using HIS.Query.Interface;
using Microsoft.AspNetCore.Http;
using Volo.Abp.DependencyInjection;

namespace ABPExample.Application.Application
{
    public class DoctorApplication:IDoctorApplication, ITransientDependency
    {
        private readonly IDoctorQuery _doctorQuery;
        private readonly IMedicalAdviceQuery _medicalAdviceQuery;
        private readonly ICommonApplication _commonApplication;
        private readonly IChatQuery _chatQuery;

        public DoctorApplication(IDoctorQuery doctorQuery,IMedicalAdviceQuery medicalAdviceQuery,ICommonApplication commonApplication,IChatQuery chatQuery)
        {
            _doctorQuery = doctorQuery;
            _medicalAdviceQuery = medicalAdviceQuery;
            _commonApplication = commonApplication;
            _chatQuery = chatQuery;
        }
        public async Task<ModelResult<List<SchedulingDto>>> GetSchedulingByUserId(int userId, DateTime? startDate, DateTime? endDate, int departmentId)
        {
            return await _doctorQuery.GetSchedulingByUserId(userId, startDate, endDate,departmentId);
        }

        public async Task<ModelResult> AddOrEditMedicalAdviceAsync(MedicalAdviceInputDto inputDto)
        {
            if (inputDto.Content.IsNullOrWhiteSpace())
                return new ModelResult {IsSuccess = false, Message = "内容不能为空"};
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

        public async Task<ModelResult> EditDoctorInfoAsync(DoctorInfoInputDto inputDto,IFormFile file)
        {
            if (file == null)
            {
                inputDto.ImgUrl = "\\Default.Png";
            }
            else
            {
                inputDto.ImgUrl = await _commonApplication.UploadFileAsync(file);
                if (inputDto.ImgUrl.IsNullOrEmpty())
                    return new ModelResult {IsSuccess = false, Message = "图片上传失败"};
            }
            return await _doctorQuery.EditDoctorInfoAsync(inputDto);

        }

        public async Task<ModelResult<DoctorEntityDto>> GetDoctorInfoDetailAsync(int userId)
        {
            var result= await _doctorQuery.GetDoctorInfoDetailAsync(userId);
            return new ModelResult<DoctorEntityDto>
                {Result = result, IsSuccess = result != null, Message = result == null ? "无效Id" : ""};
        }

        public async Task<ModelResult<List<ChatUserInfoDto>>> GetOnlineUserAsync(int doctorId)
        {
            return await _doctorQuery.GetOnlineUserAsync(doctorId);
        }

        public async Task<ModelResult<List<ChatLogDto>>> GetChatLogByUserIdAsync(int userId, int toInt)
        {
            var list = await _chatQuery.GetChatLogByDoctorIdAsync(toInt, userId, DateTime.Now.AddMonths(-1));

            foreach (var item in list)
            {
                if (item.SendFrom == 2)
                    item.IsMe = true;
                else
                {
                    item.IsMe = false;
                }
            }
            return new ModelResult<List<ChatLogDto>>
            {
                Result = list
            };
        }
    }
}
