using ABPExample.Domain.Dtos.Common;
using ABPExample.Domain.Dtos.UserDtos;
using ABPExample.Domain.Public;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ABPExample.Application.Interface
{
    public interface IUserApplication
    {
        Task<AuthUserInfoDto> Authentication(LoginInputDto inputDto) ;

        Task<bool> Register(RegisterInputDto inputDto);

        Task<ModelResult> AddUser(AddUserInputDto inputDto);

        Task<ModelResult> BatchAddUser(List<AddUserInputDto> inputDtoList);

        Task<ModelResult<PageDto<UserInfoListDto>>> GetUserInfoList(UserInfoListSearchDto inputDto);

        Task<ModelResult<UserInfoDetailDto>> GetUserInfoDetail(int id);

        Task<ModelResult> ResetUserPassWord(int id);

        Task<ModelResult> EditUserInfo(EditUserInfoDto infoDto);

    }
}
