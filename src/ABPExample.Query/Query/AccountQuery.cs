using ABPExample.Domain.Dtos.Account;
using ABPExample.Domain.Dtos.UserDtos;
using ABPExample.Domain.Models;
using ABPExample.EntityFramework.EntityFrameworkCore;
using ABPExample.Query.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ABPExample.Domain.Public;
using Volo.Abp.DependencyInjection;

namespace ABPExample.Query.Query
{
    public class AccountQuery : IAccountQuery, ITransientDependency
    {
        private readonly IAppDbContext _context;
        private readonly IHttpContextAccessor _accessor;

        public AccountQuery(IAppDbContext context, IHttpContextAccessor accessor)
        {
            _context = context;
            _accessor = accessor;
        }


        public async Task<bool> Authentication(LoginInputDto inputDto)
        {
            var info = await (
                from a in _context.Users
                from  b in _context.RoleMapper.Where(rm=>rm.UserId==a.Id&&!rm.IsDeleted).DefaultIfEmpty()
                from c in _context.Role.Where(r=>r.Id==b.RoleId&&!r.IsDeleted)
                where a.UserAccount == inputDto.AccountNo
                where !a.IsDeleted && !b.IsDeleted && !c.IsDeleted
                select new
                {
                    UserId= a.Id,
                    a.UserAccount,
                    a.UserName,
                    a.UserPwd,
                    c.Id
                }).FirstOrDefaultAsync();

            if (info == null)
                return false;

            var pwdHasher = new PasswordHasher<object>();
            var result = pwdHasher.VerifyHashedPassword(info, info.UserPwd, inputDto.PassWord);
            if (result == PasswordVerificationResult.Success)
            {
                var claims = new List<Claim>
                {
                    new Claim("Name",info.UserName),
                    new Claim("Role", info.Id.ToString()),
                    new Claim("UserId",info.UserId.ToString()),
                    new Claim("UserNo",info.UserAccount)
                };
                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {

                };

                var httpContext = _accessor.HttpContext;
                
                await httpContext.SignInAsync(
                   CookieAuthenticationDefaults.AuthenticationScheme,
                   new ClaimsPrincipal(claimsIdentity),
                   authProperties);
                return true;
            }
            else
                return false;
        }

        public async Task<ModelResult> EditPassWordAsync(EditPassWordDto inputDto)
        {
            var userInfo = await _context.Users.FirstOrDefaultAsync(c => c.Id == inputDto.Id);
            if (userInfo == null)
                return new ModelResult { IsSuccess = false, Message = "没有查询到相关信息！" };
            var passwordHasher = new PasswordHasher<Users>();
            userInfo.UserPwd = passwordHasher.HashPassword(userInfo, inputDto.NewPassWord);
            _context.Update(userInfo);
            await _context.SaveChangesAsync();
            return new ModelResult { IsSuccess = true, Message = "重置密码成功！" };
        }
    }
}
