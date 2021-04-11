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

      public PatientUserApplication(IPatientUserQuery patientUserQuery, IHttpContextAccessor accessor, IPatientQuery patientQuery)
      {
         _patientUserQuery = patientUserQuery;
         _accessor = accessor;
         _patientQuery = patientQuery;
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
                new Claim("Id",result.First().Key.ToString()),
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
            return new ModelResult {IsSuccess = false, Message = "无效Id"};
         var entity = await _patientQuery.GetPatientDetailAsync(inputDto.IdentityId);

         if (entity == null)
         {
            var result = await _patientQuery.Add(inputDto);
            if (!result.IsSuccess)
               return new ModelResult { IsSuccess = false, Message = "添加患者信息失败" };
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
   }
}
