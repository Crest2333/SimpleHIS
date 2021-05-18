using ABPExample.Domain.Dtos.Account;
using ABPExample.Domain.Dtos.UserDtos;
using ABPExample.Domain.Public;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ABPExample.Application.Interface
{
    public interface IAccountApplication
    {
        /// <summary>
        /// 身份认证
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        Task<ModelResult<bool>> Authentication(LoginInputDto inputDto);

        Task<ModelResult> EditPassWordAsync(EditPassWordDto inputDto);
    }
}
