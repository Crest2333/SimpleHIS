using ABPExample.Domain.Dtos.Common;
using ABPExample.Domain.Dtos.UserDtos;
using ABPExample.Domain.Models;
using ABPExample.Domain.Models.Enum;
using ABPExample.Domain.Public;
using ABPExample.EntityFramework.EntityFrameworkCore;
using ABPExample.Query.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace ABPExample.Query.Query
{
    public class UserQuery : IUserQuery, ITransientDependency
    {
        private readonly IAppDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public UserQuery(IAppDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }
        public async Task<AuthUserInfoDto> Authentication(LoginInputDto inputDto)
        {
            return await _context.Users
                .Where(c => c.Email == inputDto.AccountNo && c.UserPwd == inputDto.PassWord)
                .Select(c => new AuthUserInfoDto
                {
                    UserName = c.UserName,
                    UserId = c.Id,
                }).FirstOrDefaultAsync();
        }

        public async Task<bool> Register(RegisterInputDto inputDto)
        {
            var uniqueFileName = string.Empty;
            var uploadFolder = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot/images");
            uniqueFileName = Guid.NewGuid().ToString() + "_" + inputDto.Img.FileName;
            var filePath = Path.Combine(uploadFolder, uniqueFileName);
            inputDto.Img.CopyTo(new FileStream(filePath, FileMode.Create));
            var pwd = inputDto.Identity.Substring(inputDto.Identity.Length - 4);
            var passwordHasher = new PasswordHasher<Users>();

            var personnel = new Users
            {
                UserName = inputDto.Name,
                PhoneNumber = inputDto.Phone,
                IsDeleted = false,
                LastModificationTime = DateTime.Now,
                CreationTime = DateTime.Now,
                UserImg = filePath,
                UserAccount = DateTime.UtcNow.ToLocalTime() + inputDto.Identity.Substring(inputDto.Identity.Length - 4),
            };
            var userPwd = passwordHasher.HashPassword(personnel, pwd);
            personnel.UserPwd = userPwd;
            await _context.AddAsync(personnel);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task AddUser(Users model)
        {
            var query = await _context.Users.OrderByDescending(c=>c.Id).Select(c=>c.Id).FirstOrDefaultAsync();
            model.UserAccount = string.Format("{0:d6}", query + 1);
            await _context.AddAsync(model);
            await _context.SaveChangesAsync();
        }

        public async Task BatchAddUser(List<Users> modelList)
        {
            var query = await _context.Users.OrderByDescending(c => c.Id).Select(c => c.Id).FirstOrDefaultAsync();
            var i = 1;
            modelList.ForEach(item =>
            {
                item.UserAccount = string.Format("{0:d6}", query + i++);
            });
            await _context.AddRangeAsync(modelList);
            await _context.SaveChangesAsync();
        }

        public async Task<PageDto<UserInfoListDto>> GetUserInfoList(UserInfoListSearchDto inputDto)
        {
            var query = from a in _context.Users
                        where !a.IsDeleted
                        select new UserInfoListDto
                        {
                            Email = a.Email,
                            Gender =(EnumGender) a.Gender,
                            PhoneNumber = a.PhoneNumber,
                            UserAccount = a.UserAccount,
                            UserIdentity = a.UserIdentity,
                            UserName = a.UserName,
                        };
            var count = await query.CountAsync();
            if (inputDto.PageIndex > 0 && inputDto.PageSize > 0)
                query = query.Skip((inputDto.PageIndex) - 1).Take(inputDto.PageSize);
            var list = await query.ToListAsync();

            return new PageDto<UserInfoListDto> { Count = count, List = list };
        }

        public async Task<UserInfoDetailDto> GetUserInfoDetail(int id)
        {
            var info = await _context.Users.Where(c => c.Id == id && !c.IsDeleted).Select(c => new UserInfoDetailDto
            {
                Id = (int)c.Id,
                Email = c.Email,
                Gender = c.Gender,
                PhoneNumber = c.PhoneNumber,
                UserAccount = c.UserAccount,
                UserIdentity = c.UserIdentity,
                UserImg = c.UserImg,
                UserName = c.UserName,
            }).FirstOrDefaultAsync();

            return info;
        }

        public async Task ResetUserPassWord(int id)
        {
            throw new NotImplementedException();
        }

        public async Task EditUserInfo(EditUserInfoDto infoDto)
        {
            throw new NotImplementedException();
        }
    }
}
