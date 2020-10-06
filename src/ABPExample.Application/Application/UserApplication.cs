using ABPExample.Application.Interface;
using ABPExample.Domain.Dtos.Common;
using ABPExample.Domain.Dtos.UserDtos;
using ABPExample.Domain.Models;
using ABPExample.Domain.Public;
using ABPExample.Query.Interface;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
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
                Gender = (int)inputDto.Gender,
                IsDeleted = false,
                LastModificationTime = DateTime.Now,
                Email = inputDto.Email,
                PhoneNumber = inputDto.PhoneNumber,
                UserIdentity = inputDto.Identity,
                UserName = inputDto.Name,
                UserImg="default.png",
            };
            query.UserPwd = passwordHasher.HashPassword(query, inputDto.Identity.Substring(inputDto.Identity.Length - 4));
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
                    Gender = (int)inputDto.Gender,
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
            var result= await _query.GetUserInfoList(inputDto);
            return new ModelResult<PageDto<UserInfoListDto>> { Code = 200, IsSuccess = true, Result = result };
        }

        public async Task<ModelResult<UserInfoDetailDto>> GetUserInfoDetail(int id)
        {
            var result= await _query.GetUserInfoDetail(id);
            return new ModelResult<UserInfoDetailDto> { Code = 200, IsSuccess = true, Result = result };
        }

        public async Task<ModelResult> ResetUserPassWord(int id)
        {
            await _query.ResetUserPassWord(id);
            return new ModelResult { Code = 200, IsSuccess = true };
        }

        public async Task<ModelResult> EditUserInfo(EditUserInfoDto infoDto)
        {
            await _query.EditUserInfo(infoDto);
            return new ModelResult{ Code = 200, IsSuccess = true };
        }
    }
}
