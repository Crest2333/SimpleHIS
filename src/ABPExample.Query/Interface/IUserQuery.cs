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

        Task<ModelResult> ResetUserPassWord(int id);

        Task<ModelResult> EditUserInfo(EditUserInfoDto infoDto);

        Task<ModelResult> SetUserRole(string userId, int roleId);

        Task<ModelResult> DeleteUserRole(int id);

        Task<ModelResult> EditUserPassWord(EditPassWordDto inputDto);

        Task<ModelResult> DeleteUser(long id);

        Task<ModelResult> BatchDeleteUser(List<long> idList);

        Task AddTest();

        Task<List<TestExport>> GetTestExportList();

        Task<List<UseInfo>> GetUserInfoAsync(IReadOnlyCollection<string> userNoList);

        Task<PageDto<UserInfoListDto>> GetUserRoleListAsync(UserInfoListSearchDto inputDto);

        Task<ModelResult<UseInfo>> GetUserInfoByUserNoAsync(string userNo);
    }
}
