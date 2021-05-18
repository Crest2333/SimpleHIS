using ABPExample.Application.Interface;
using ABPExample.Domain.Dtos.Account;
using ABPExample.Domain.Dtos.UserDtos;
using ABPExample.Domain.Public;
using ABPExample.Query.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABPExample.Query.Common;
using Microsoft.AspNetCore.Http;
using Volo.Abp.DependencyInjection;

namespace ABPExample.Application.Application
{
    public class AccountApplication : IAccountApplication, ITransientDependency
    {
        private readonly IAccountQuery _query;
        private readonly IHttpContextAccessor _httpContext;

        public AccountApplication(IAccountQuery query,IHttpContextAccessor httpContext)
        {
            _query = query;
            _httpContext = httpContext;
        }
        public async Task<ModelResult<bool>> Authentication(LoginInputDto inputDto)
        {
            if(await _query.Authentication(inputDto))
            {
                return new ModelResult<bool> { Code = 200, IsSuccess = true, Result = true, Message = "登录成功" };
            }
            else
            {
                return new ModelResult<bool> { Code = 200, IsSuccess = false, Result = false, Message = "账号或密码错误" };
            }
        }

        public async Task<ModelResult> EditPassWordAsync(EditPassWordDto inputDto)
        {
            if(inputDto.NewPassWord.IsNullOrWhiteSpace())
                return ModelResult.Instance.Error("请输入密码");

            if(inputDto.ConfirmPassWord.IsNullOrWhiteSpace())
                return ModelResult.Instance.Error("请输入确认密码");

            if (inputDto.NewPassWord != inputDto.ConfirmPassWord)
            {
                return ModelResult.Instance.Error("两次密码不相同");
            }

            inputDto.Id = _httpContext.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId").Value.ToInt();

            return await _query.EditPassWordAsync(inputDto);
        }
    }
}
