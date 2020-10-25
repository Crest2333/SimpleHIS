using ABPExample.Application.Interface;
using ABPExample.Domain.Dtos.Account;
using ABPExample.Domain.Dtos.UserDtos;
using ABPExample.Domain.Public;
using ABPExample.Query.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace ABPExample.Application.Application
{
    public class AccountApplication : IAccountApplication, ITransientDependency
    {
        private readonly IAccountQuery _query;

        public AccountApplication(IAccountQuery query)
        {
           _query = query;
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
    }
}
