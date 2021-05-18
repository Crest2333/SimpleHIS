using ABPExample.Application.Common;
using ABPExample.Application.Interface;
using ABPExample.Domain.Dtos.Common;
using ABPExample.Domain.Dtos.UserDtos;
using ABPExample.Domain.Models;
using ABPExample.Domain.Public;
using ABPExample.Query.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Volo.Abp.DependencyInjection;

namespace ABPExample.Application.Application
{
    public class UserApplication : IUserApplication, ITransientDependency
    {
        private readonly IUserQuery _query;
        private readonly IMemoryCache _memoryCache;

        public UserApplication(IUserQuery query, IMemoryCache memoryCache)
        {
            _query = query;
            _memoryCache = memoryCache;
        }
        public async Task<AuthUserInfoDto> Authentication(LoginInputDto inputDto)
        {
            return await _query.Authentication(inputDto);
        }

        public async Task<bool> Register(RegisterInputDto inputDto)
        {
            return await _query.Register(inputDto);
        }

        public async Task<ModelResult> AddUser(AddUserInputDto inputDto)
        {
            if (inputDto.Name.IsNullOrWhiteSpace())
                return new ModelResult { IsSuccess = false, Message = "名称不能为空" };

            var passwordHasher = new PasswordHasher<Users>();

            if (inputDto.Identity.Length != 18)
                return new ModelResult { IsSuccess = false, Message = "身份证号格式错误" };

            var regEmail = new Regex("^[\\w-]+@[\\w-]+\\.(com|net|org|edu|mil|tv|biz|info)$");
            if (!regEmail.Match(inputDto.Email).Success)
                return new ModelResult { IsSuccess = false, Message = "邮箱格式错误" };

            var rexPhone = new Regex(@"^\d+$");
            if (!rexPhone.IsMatch(inputDto.PhoneNumber))
                return new ModelResult { IsSuccess = false, Message = "电话必须为数字" };

            var list = await _query.GetUserInfoList(new UserInfoListSearchDto
            { PageIndex = 1, PageSize = 10, IdentityId = inputDto.Identity });
            if (list.Count > 0)
                return new ModelResult { IsSuccess = false, Message = "身份证号重复" };

            list = await _query.GetUserInfoList(new UserInfoListSearchDto
            { PageIndex = 1, PageSize = 10, PhoneNumber = inputDto.PhoneNumber });
            if (list.Count > 0)
                return new ModelResult { IsSuccess = false, Message = "手机号重复" };

            list = await _query.GetUserInfoList(new UserInfoListSearchDto
            { PageIndex = 1, PageSize = 10, Email = inputDto.Email });
            if (list.Count > 0)
                return new ModelResult { IsSuccess = false, Message = "邮箱重复" };

            var query = new Users
            {
                CreationTime = DateTime.Now,
                Gender = (Gender)inputDto.Gender,
                IsDeleted = false,
                LastModificationTime = DateTime.Now,
                Email = inputDto.Email,
                PhoneNumber = inputDto.PhoneNumber,
                UserIdentity = inputDto.Identity,
                UserName = inputDto.Name,
                UserImg = "default.png",
            };
            query.UserPwd = passwordHasher.HashPassword(query, inputDto.Identity.Substring(inputDto.Identity.Length - 6));
            await _query.AddUser(query);

            return new ModelResult { Code = 200, IsSuccess = true, Message = "添加成功" };
        }

        public async Task<ModelResult<UseInfo>> GetUserInfoByUserNoAsync(string userNo)
        {
            return await _query.GetUserInfoByUserNoAsync(userNo);
        }

        public async Task<ModelResult> BatchAddUser(List<AddUserInputDto> inputDtoList)
        {
            var list = new List<Users>();
            var passwordHasher = new PasswordHasher<Users>();
            inputDtoList.ForEach(inputDto =>
            {
                var query = new Users
                {
                    CreationTime = DateTime.Now,
                    Gender = (Gender)inputDto.Gender,
                    IsDeleted = false,
                    LastModificationTime = DateTime.Now,
                    Email = inputDto.Email,
                    PhoneNumber = inputDto.PhoneNumber,
                    UserIdentity = inputDto.Identity,
                    UserName = inputDto.Name,
                    UserImg = "default.png",
                };
                query.UserPwd = passwordHasher.HashPassword(query, inputDto.Identity.Substring(inputDto.Identity.Length - 6));
                list.Add(query);
            });

            await _query.BatchAddUser(list);
            return new ModelResult { Code = 200, IsSuccess = true, Message = "添加成功" };

        }

        public async Task<ModelResult<PageDto<UserInfoListDto>>> GetUserInfoList(UserInfoListSearchDto inputDto)
        {
            var result = (inputDto.IsRoleUser.HasValue && inputDto.IsRoleUser.Value) ? await _query.GetUserRoleListAsync(inputDto) : await _query.GetUserInfoList(inputDto);
            return new ModelResult<PageDto<UserInfoListDto>> { Code = 200, IsSuccess = true, Result = result };
        }

        public async Task<ModelResult<PageDto<UserInfoListDto>>> GetUserRoleListAsync(UserInfoListSearchDto inputDto)
        {
            var result = await _query.GetUserRoleListAsync(inputDto);
            return new ModelResult<PageDto<UserInfoListDto>> { Code = 200, IsSuccess = true, Result = result };
        }

        public async Task<ModelResult<UserInfoDetailDto>> GetUserInfoDetail(int id)
        {
            var result = await _memoryCache.GetOrCreateAsync<UserInfoDetailDto>(id, async item => await _query.GetUserInfoDetail(id));

            return new ModelResult<UserInfoDetailDto> { Code = 200, IsSuccess = true, Result = result };
        }

        public async Task<ModelResult> ResetUserPassWord(int id)
        {
            return await _query.ResetUserPassWord(id);
        }

        public async Task<ModelResult> EditUserInfo(EditUserInfoDto inputDto)
        {
            if (inputDto.Name.IsNullOrWhiteSpace())
                return new ModelResult { IsSuccess = false, Message = "名称不能为空" };

            var passwordHasher = new PasswordHasher<Users>();

            if (inputDto.Identity.Length != 18)
                return new ModelResult { IsSuccess = false, Message = "身份证号格式错误" };

            var regEmail = new Regex("^[\\w-]+@[\\w-]+\\.(com|net|org|edu|mil|tv|biz|info)$");
            if (!regEmail.Match(inputDto.Email).Success)
                return new ModelResult { IsSuccess = false, Message = "邮箱格式错误" };

            var rexPhone = new Regex(@"^\d+$");
            if (!rexPhone.IsMatch(inputDto.PhoneNumber))
                return new ModelResult { IsSuccess = false, Message = "电话必须为数字" };
            return await _query.EditUserInfo(inputDto);
        }

        public async Task<ModelResult<DataTable>> BatchAddUser(IFormFile file)
        {

            if (file.Length == 0)
                return new ModelResult<DataTable> { IsSuccess = false, Message = "请上传一个文件" };
            var data = ExcelHelper.ToDataTable(file);
            data.Columns.Add("错误信息");
            var error = data.Clone();
            var list = new List<AddUserInputDto>();

            for (var i = 0; i < data.Rows.Count; i++)
            {
                var item = new AddUserInputDto();
                if (string.IsNullOrWhiteSpace(data.Rows[i]["姓名"].ToString())
                || string.IsNullOrWhiteSpace(data.Rows[i]["电话"].ToString())
                || string.IsNullOrWhiteSpace(data.Rows[i]["邮箱"].ToString())
                || string.IsNullOrWhiteSpace(data.Rows[i]["身份证号"].ToString())
               )
                {
                    data.Rows[i]["错误信息"] = "请将数据填写完整";
                    error.Rows.Add(data.Rows[i].ItemArray);
                    continue;
                }

                item.Name = data.Rows[i]["姓名"].ToString();
                switch (data.Rows[i]["性别"].ToString())
                {
                    case "男":
                        item.Gender = Domain.Models.Enum.EnumGender.Man;
                        break;
                    case "女":
                        item.Gender = Domain.Models.Enum.EnumGender.Woman;
                        break;
                    case "其他":
                        item.Gender = Domain.Models.Enum.EnumGender.Other;
                        break;
                    default:
                        data.Rows[i]["错误信息"] = "性别格式错误";
                        error.Rows.Add(data.Rows[i].ItemArray);
                        continue;
                }

                item.PhoneNumber = data.Rows[i]["电话"].ToString();
                item.Email = data.Rows[i]["邮箱"].ToString();
                item.Identity = data.Rows[i]["身份证号"].ToString();
                if (item.Identity.Length != 18)
                {
                    data.Rows[i]["错误信息"] = "身份证号格式错误";
                    error.Rows.Add(data.Rows[i].ItemArray);
                    continue;
                }

                var regEmail = new Regex("^[\\w-]+@[\\w-]+\\.(com|net|org|edu|mil|tv|biz|info)$");
                if (!regEmail.Match(item.Email).Success)
                {
                    data.Rows[i]["错误信息"] = "邮箱格式错误";
                    error.Rows.Add(data.Rows[i].ItemArray);
                    continue;
                }

                var rexPhone = new Regex(@"^\d+$");
                if (!rexPhone.IsMatch(item.PhoneNumber))
                {
                    data.Rows[i]["错误信息"] = "电话必须为数字";
                    error.Rows.Add(data.Rows[i].ItemArray);
                    continue;
                }

                var userList = await _query.GetUserInfoList(new UserInfoListSearchDto
                { PageIndex = 1, PageSize = 10, IdentityId = item.Identity });
                if (userList.Count > 0)
                {
                    data.Rows[i]["错误信息"] = "身份证号重复";
                    error.Rows.Add(data.Rows[i].ItemArray);
                    continue;
                }

                userList = await _query.GetUserInfoList(new UserInfoListSearchDto
                { PageIndex = 1, PageSize = 10, PhoneNumber = item.PhoneNumber });
                if (userList.Count > 0)
                {
                    data.Rows[i]["错误信息"] = "手机号重复";
                    error.Rows.Add(data.Rows[i].ItemArray);
                    continue;
                }

                userList = await _query.GetUserInfoList(new UserInfoListSearchDto
                { PageIndex = 1, PageSize = 10, Email = item.Email });
                if (userList.Count > 0)
                {
                    data.Rows[i]["错误信息"] = "邮箱重复";
                    error.Rows.Add(data.Rows[i].ItemArray);
                    continue;
                }


                list.Add(item);
            }

            if (list.Any())
                await BatchAddUser(list);
            return new ModelResult<DataTable> { Result = error };
        }



        public async Task<ModelResult> SetUserRole(string userId, int roleId)
        {
            return await _query.SetUserRole(userId, roleId);
        }

        public async Task<ModelResult> DeleteUserRole(int userId)
        {
            return await _query.DeleteUserRole(userId);
        }

        public Task<ModelResult<string>> BatchEditUserInfo(List<EditUserInfoDto> infoDtoList)
        {
            throw new NotImplementedException();
        }

        public async Task<ModelResult> EditUserPassWord(EditPassWordDto inputDto)
        {
            return await _query.EditUserPassWord(inputDto);
        }

        public async Task<ModelResult> DeleteUser(long id)
        {

            return await _query.DeleteUser(id);
        }

        public async Task<ModelResult> BatchDeleteUser(List<long> idList)
        {
            return await _query.BatchDeleteUser(idList);
        }

        public async Task<List<TestExport>> GetTestExportListAsync()
        {
            return await _query.GetTestExportList();
        }

        public async Task<Stream> ExportUserInfo()
        {
            var list = await GetTestExportListAsync();
            var dt = list.ToDataTable();
            return ExcelHelper.Export(dt);
        }
    }
}
