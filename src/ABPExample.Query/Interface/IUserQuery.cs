using ABPExample.Domain.Dtos.Common;
using ABPExample.Domain.Dtos.UserDtos;
using ABPExample.Domain.Models;
using ABPExample.Domain.Public;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ABPExample.Query.Interface
{
    public interface IUserQuery
    {
        Task<AuthUserInfoDto> Authentication(LoginInputDto inputDto);

        Task<bool> Register(RegisterInputDto inputDto);

        Task AddUser(Users model);

        Task BatchAddUser(List<Users> model);

        Task<PageDto<UserInfoListDto>> GetUserInfoList(UserInfoListSearchDto inputDto);

        Task<UserInfoDetailDto> GetUserInfoDetail(int id);

        Task ResetUserPassWord(int id);

        Task EditUserInfo(EditUserInfoDto infoDto);
    }
}
