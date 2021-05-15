using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ABPExample.Domain.Dtos.Patient;
using ABPExample.Domain.Dtos.UserDtos;
using ABPExample.Domain.Public;
using ABPExample.Query.Interface;
using HIS.Application.Interface;
using HIS.Domain.Dtos.Chat;
using HIS.Query.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;
using Volo.Abp.DependencyInjection;
using RegisterInputDto = HIS.Domain.Dtos.PatientUser.RegisterInputDto;

namespace HIS.Application.Application
{
    public class PatientUserApplication : IPatientUserApplication, ITransientDependency
    {
        private readonly IPatientUserQuery _patientUserQuery;
        private readonly IHttpContextAccessor _accessor;
        private readonly IPatientQuery _patientQuery;
        private readonly IChatQuery _chatQuery;

        public PatientUserApplication(IPatientUserQuery patientUserQuery, IHttpContextAccessor accessor, IPatientQuery patientQuery, IChatQuery chatQuery)
        {
            _patientUserQuery = patientUserQuery;
            _accessor = accessor;
            _patientQuery = patientQuery;
            _chatQuery = chatQuery;
        }


        public async Task<ModelResult> RegisterAsync(RegisterInputDto input)
        {
            var regEmail = new Regex("^[\\w-]+@[\\w-]+\\.(com|net|org|edu|mil|tv|biz|info)$");
            if (!regEmail.Match(input.Email).Success)
                return new ModelResult { IsSuccess = false, Message = "邮箱格式错误" };

            var rexPhone = new Regex(@"^\d+$");
            if (!rexPhone.IsMatch(input.PhoneNumber))
                return new ModelResult { IsSuccess = false, Message = "电话必须为数字" };

            return new ModelResult { IsSuccess = await _patientUserQuery.RegisterAsync(input) };

        }

        public async Task<ModelResult> LoginAsync(LoginInputDto input)
        {
            var result = await _patientUserQuery.AuthenticationAsync(input.AccountNo, input.PassWord);

            if (result == null) return new ModelResult { IsSuccess = false, Message = "密码错误" };

            var claims = new List<Claim>
            {
                new Claim("UserId",result.First().Key.ToString()),
                new Claim("Name", result.First().Value),
            };
            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {

            };

            var httpContext = _accessor.HttpContext;

            await httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
            return new ModelResult { IsSuccess = true };
        }

        public async Task<ModelResult> AddPatientMapperAsync(AddPatientInfoDto inputDto, int userId)
        {
            if (userId == 0)
                return new ModelResult { IsSuccess = false, Message = "无效Id" };
            var entity = await _patientQuery.GetPatientDetailAsync(inputDto.IdentityId);

            if (entity == null)
            {
                var result = await _patientQuery.Add(inputDto);
                if (!result.IsSuccess)
                    return new ModelResult { IsSuccess = false, Message = result.Message };
                if (!await _patientUserQuery.AddPatientMapperAsync(userId, result.Result))
                    return new ModelResult { IsSuccess = false, Message = "添加患者信息失败" };
            }
            else
            {
                var result = await _patientUserQuery.AddPatientMapperAsync(userId, entity.Id);
                if (!result)
                    return new ModelResult { IsSuccess = false, Message = "添加患者信息失败" };

            }
            return new ModelResult { IsSuccess = true };
        }

        public async Task<ModelResult<List<ChatLogDto>>> GetChatLogByDoctorIdAsync(int doctorId, int patientUserId, DateTime? startDateTime)
        {
            var list = await _chatQuery.GetChatLogByDoctorIdAsync(doctorId, patientUserId, startDateTime);

            foreach (var item in list)
            {
                if (item.SendFrom == 1)
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

        public async Task<ModelResult<bool>> AddAsync(string message, int patientUserId, int doctorId,int from)
        {
            if (message.IsNullOrWhiteSpace())
                return new ModelResult<bool> {IsSuccess = false, Message = "不能输入空格"};

            return new ModelResult<bool> {IsSuccess = await _chatQuery.AddAsync(message, patientUserId, doctorId,from)};
        }
    }
}
