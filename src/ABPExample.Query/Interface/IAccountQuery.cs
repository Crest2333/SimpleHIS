using ABPExample.Domain.Dtos.Account;
using ABPExample.Domain.Dtos.UserDtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ABPExample.Query.Interface
{
    public interface IAccountQuery
    {
        /// <summary>
        /// 身份认证
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        Task<bool> Authentication(LoginInputDto inputDto);
    }
}
