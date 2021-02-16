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
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace ABPExample.Application.Application
{
    public class UserApplication : IUserApplication, ITransientDependency
    {
        private readonly IUserQuery _query;

        public UserApplication(IUserQuery query)
        {
            _query = query;
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
            var passwordHasher = new PasswordHasher<Users>();
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
                query.UserPwd = passwordHasher.HashPassword(query, inputDto.Identity.Substring(inputDto.Identity.Length - 4));
                list.Add(query);
            });

            await _query.BatchAddUser(list);
            return new ModelResult { Code = 200, IsSuccess = true, Message = "添加成功" };

        }

        public async Task<ModelResult<PageDto<UserInfoListDto>>> GetUserInfoList(UserInfoListSearchDto inputDto)
        {
            var result = await _query.GetUserInfoList(inputDto);
            return new ModelResult<PageDto<UserInfoListDto>> { Code = 200, IsSuccess = true, Result = result };
        }

        public async Task<ModelResult<UserInfoDetailDto>> GetUserInfoDetail(int id)
        {
            var result = await _query.GetUserInfoDetail(id);
            return new ModelResult<UserInfoDetailDto> { Code = 200, IsSuccess = true, Result = result };
        }

        public async Task<ModelResult> ResetUserPassWord(int id)
        {
            return await _query.ResetUserPassWord(id);
        }

        public async Task<ModelResult> EditUserInfo(EditUserInfoDto infoDto)
        {
            return await _query.EditUserInfo(infoDto);
        }

        public async Task<ModelResult> BatchAddUser(IFormFile file)
        {
            if (file.Length == 0)
                return new ModelResult { IsSuccess = false, Message = "请上传一个文件" };
            var data = ExcelHelper.ToDataTable(file);

            var list = new List<AddUserInputDto>();

            for (var i = 0; i < data.Rows.Count; i++)
            {
                var item = new AddUserInputDto();
                if (string.IsNullOrWhiteSpace(data.Rows[i]["姓名"].ToString()))
                    return new ModelResult { IsSuccess = false, Message = "请将数据填写完整" };
                if (string.IsNullOrWhiteSpace(data.Rows[i]["性别"].ToString()))
                    return new ModelResult { IsSuccess = false, Message = "请将数据填写完整" };
                if (string.IsNullOrWhiteSpace(data.Rows[i]["电话"].ToString()))
                    return new ModelResult { IsSuccess = false, Message = "请将数据填写完整" };
                if (string.IsNullOrWhiteSpace(data.Rows[i]["邮箱"].ToString()))
                    return new ModelResult { IsSuccess = false, Message = "请将数据填写完整" };
                if (string.IsNullOrWhiteSpace(data.Rows[i]["身份证号"].ToString()))
                    return new ModelResult { IsSuccess = false, Message = "请将数据填写完整" };

                item.Name = data.Rows[i]["姓名"].ToString();
                switch (data.Rows[i]["性别"].ToString())
                {
                    case "男":
                        item.Gender = Domain.Models.Enum.EnumGender.man;
                        break;
                    case "女":
                        item.Gender = Domain.Models.Enum.EnumGender.woman;
                        break;
                    case "其他":
                        item.Gender = Domain.Models.Enum.EnumGender.other;
                        break;
                    default:
                        return new ModelResult { IsSuccess = false, Message = "性别字段数据格式错误" };
                }

                item.PhoneNumber = data.Rows[i]["电话"].ToString();
                item.Email = data.Rows[i]["邮箱"].ToString();
                item.Identity = data.Rows[i]["身份证号"].ToString();

                list.Add(item);
            }
            return await BatchAddUser(list);

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
            for(var index = 1; index <= 60000; index++)
            {
                await _query.AddTest();
            }
            return null;
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
            return ExcelHelper.ToExcel(dt, "Test", type: ExcelHelper.ExcelType.XLSX);
        }
    }
}
