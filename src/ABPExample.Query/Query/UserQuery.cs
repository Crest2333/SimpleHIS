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
            var query = await _context.Users.OrderByDescending(c => c.Id).Select(c => c.Id).FirstOrDefaultAsync();
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
                            Gender = (EnumGender)a.Gender,
                            PhoneNumber = a.PhoneNumber,
                            UserAccount = a.UserAccount,
                            UserIdentity = a.UserIdentity,
                            UserName = a.UserName,
                        };
            var count = await query.CountAsync();
            if (inputDto.PageIndex > 0 && inputDto.PageSize > 0)
                query = query.Skip((inputDto.PageIndex) - 1).Take(inputDto.PageSize);
            var list = await query.ToListAsync();

            return new PageDto<UserInfoListDto>(count, list);
        }

        public async Task<UserInfoDetailDto> GetUserInfoDetail(int id)
        {
            var info = await _context.Users.Where(c => c.Id == id && !c.IsDeleted).Select(c => new UserInfoDetailDto
            {
                Id = (int)c.Id,
                Email = c.Email,
                Gender =(int) c.Gender,
                PhoneNumber = c.PhoneNumber,
                UserAccount = c.UserAccount,
                UserIdentity = c.UserIdentity,
                UserImg = c.UserImg,
                UserName = c.UserName,
            }).FirstOrDefaultAsync();

            return info;
        }

        public async Task<ModelResult> ResetUserPassWord(int id)
        {
            var passwordHasher = new PasswordHasher<Users>();
            var userInfo = await _context.Users.FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
            if (userInfo == null)
                return new ModelResult { IsSuccess = false, Message = "没有查询到相关信息！" };
            userInfo.UserPwd = passwordHasher.HashPassword(userInfo, userInfo.UserIdentity.Substring(userInfo.UserIdentity.Length - 6));
            _context.Update(userInfo);
            await _context.SaveChangesAsync();
            return new ModelResult { IsSuccess = true, Message = "重置密码成功！" };
        }

        public async Task<ModelResult> EditUserInfo(EditUserInfoDto infoDto)
        {
            var userInfo = await _context.Users.FirstOrDefaultAsync(c => c.Id == infoDto.Id);
            if (userInfo == null)
                return new ModelResult { IsSuccess = false, Message = "没有查询到相关信息！" };
            _context.Update(userInfo);
            await _context.SaveChangesAsync();
            return new ModelResult { IsSuccess = true, Message = "修改成功" };
        }

        public async Task<ModelResult> EditUserPassWord(EditPassWordDto inputDto)
        {
            var passwordHasher = new PasswordHasher<Users>();
            var userInfo = await _context.Users.FirstOrDefaultAsync(c => c.Id == inputDto.Id && !c.IsDeleted);
            if (userInfo == null)
                return new ModelResult { IsSuccess = false, Message = "没有查询到相关信息！" };

            var result = passwordHasher.VerifyHashedPassword(userInfo, userInfo.UserPwd, inputDto.OldPassWord);
            if (result != PasswordVerificationResult.Success)
                return new ModelResult { IsSuccess = false, Message = "密码错误！" };

            userInfo.UserPwd = passwordHasher.HashPassword(userInfo, inputDto.NewPassWord);
            _context.Update(userInfo);
            await _context.SaveChangesAsync();
            return new ModelResult { IsSuccess = true, Message = "修改密码成功密码成功！" };
        }

        public async Task<ModelResult> SetUserRole(string userId, int roleId)
        {
            var query = await _context.RoleMapper.FirstOrDefaultAsync(c => c.AccountNo == userId && c.RoleId == roleId && !c.IsDeleted);
            if (query != null)
                return new ModelResult { IsSuccess = false, Message = "添加失败，该账号已有一个角色" };
            var roleMapper = new RoleMapper
            {
                AccountNo = userId,
                CreationTime = DateTime.Now,
                IsDeleted = false,
                LastModificationTime = DateTime.Now,
                RoleId = roleId
            };
            await _context.AddAsync(roleMapper);
            await _context.SaveChangesAsync();
            return new ModelResult { IsSuccess = true, Code = 200, Message = "添加成功" };
        }

        public async Task<ModelResult> DeleteUserRole(int id)
        {
            var query = await _context.RoleMapper.FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
            if (query == null)
                return new ModelResult { IsSuccess = false, Message = "没有找到相关信息！" };
            query.IsDeleted = true;
            _context.Update(query);
            await _context.SaveChangesAsync();
            return new ModelResult { IsSuccess = true, Code = 200, Message = "删除成功！" };
        }

        public async Task<ModelResult> DeleteUser(long id)
        {
            var userInfo = await _context.Users.FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
            if (userInfo == null)
                return new ModelResult { IsSuccess = false, Message = "没有找到相关信息！" };
            userInfo.IsDeleted = true;
            _context.Update(userInfo);
            await _context.SaveChangesAsync();
            return new ModelResult { IsSuccess = true, Code = 200, Message = "删除成功！" };
        }

        public async Task<ModelResult> BatchDeleteUser(List<long> idList)
        {
            var userInfoList = await _context.Users
                .Where(c => idList.Contains(c.Id) && !c.IsDeleted)
                .Select(c => new Users
                {
                    Id = c.Id,
                    IsDeleted = true
                }).ToListAsync();
            if (!userInfoList.Any())
                return new ModelResult { IsSuccess = false, Message = "没有找到相关信息！" };

            foreach (var item in userInfoList)
            {
                _context.Attach(item);//告诉EF Core开始跟踪person实体的更改，因为调用DbContext.Attach方法后，EF Core会将person实体的State值（可以通过testDBContext.Entry(person).State查看到）更改回EntityState.Unchanged，所以这里testDBContext.Attach(person)一定要放在下面一行testDBContext.Entry(person).Property(p => p.Name).IsModified = true的前面，否者后面的testDBContext.SaveChanges方法调用后，数据库不会被更新
                _context.Entry(item).Property(p => p.IsDeleted).IsModified = true;//告诉EF Core实体person的Name属性已经更改。将testDBContext.Entry(person).Property(p => p.Name).IsModified设置为true后，也会将person实体的State值（可以通过testDBContext.Entry(person).State查看到）更改为EntityState.Modified，这样就保证了下面SaveChanges的时候会将person实体的Name属性值Update到数据库中。
            }
            await _context.SaveChangesAsync();
            return new ModelResult { IsSuccess = true, Code = 200, Message = "删除成功！" };
        }

        public async Task AddTest()
        {
            TestExport testExport = new TestExport
            {
                A1 = "qwe123",
                A2 = "qwe123",
                A3 = "qwe123",
                A4 = "qwe123",
                A5 = "qwe123",
                A6 = "qwe123",
                A7 = "qwe123",
                A8 = "qwe123",
                A9 = "qwe123",
                A10 = "qwe123",
                A11 = "qwe123",
                A12 = "qwe123",
                A13 = "qwe123",
                A14 = "qwe123",
                A16 = "qwe123",
                A15 = "qwe123",
                A17 = "qwe123",
                A18 = "qwe123",
                A19 = "qwe123",
                A20 = "qwe123",
                A21 = "qwe123",
                A22 = "qwe123",
                A23 = "qwe123",
                A24 = "qwe123",
                A25 = "qwe123",
                A26 = "qwe123",
                A27 = "qwe123",
                A28 = "qwe123",
                A29 = "qwe123",
                A30 = "qwe123",
                A31 = "qwe123",
                A32 = "qwe123",
                A33 = "qwe123",
                A34 = "qwe123",
                A35 = "qwe123"
            };
            await _context.AddAsync(testExport);
            await _context.SaveChangesAsync();
        }

        public async Task<List<TestExport>> GetTestExportList()
        {
            return await  _context.TestExport.ToListAsync();
        }
    }
}
