using ABPExample.Domain.Dtos.Common;
using ABPExample.Domain.Dtos.UserDtos;
using ABPExample.Domain.Models;
using ABPExample.Domain.Public;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ABPExample.Application.Interface
{
    public interface IUserApplication
    {
        Task<AuthUserInfoDto> Authentication(LoginInputDto inputDto);

        Task<bool> Register(RegisterInputDto inputDto);

        Task<ModelResult> AddUser(AddUserInputDto inputDto);

        Task<ModelResult<UseInfo>> GetUserInfoByUserNoAsync(string userNo);

        Task<ModelResult> BatchAddUser(List<AddUserInputDto> inputDtoList);

        Task<ModelResult<DataTable>> BatchAddUser(IFormFile file);

        Task<ModelResult<PageDto<UserInfoListDto>>> GetUserInfoList(UserInfoListSearchDto inputDto);

        Task<ModelResult<PageDto<UserInfoListDto>>> GetUserRoleListAsync(UserInfoListSearchDto inputDto);


        Task<ModelResult<UserInfoDetailDto>> GetUserInfoDetail(int id);

        Task<ModelResult> ResetUserPassWord(int id);

        Task<ModelResult> EditUserInfo(EditUserInfoDto infoDto);

        Task<ModelResult> SetUserRole(string userId, int roleId);

        Task<ModelResult> DeleteUserRole(int userId);

        Task<ModelResult<string>> BatchEditUserInfo(List<EditUserInfoDto> infoDtoList);

        Task<ModelResult> EditUserPassWord(EditPassWordDto inputDto);

        Task<ModelResult> DeleteUser(long id);

        Task<ModelResult> BatchDeleteUser(List<long> idList);

        Task<List<TestExport>> GetTestExportListAsync();

        Task<Stream> ExportUserInfo();


    }
}
