using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABPExample.Domain.Dtos.UserDtos;
using ABPExample.Domain.Public;
using HIS.Application.Interface;
using HIS.Domain.Dtos.PatientUser;
using HIS.Query.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Volo.Abp.AspNetCore.Mvc;
using Web.Common;
using RegisterInputDto = HIS.Domain.Dtos.PatientUser.RegisterInputDto;

namespace Web.Controllers
{
    public class PatientAccountController : AbpController
    {
        private readonly IPatientUserApplication _patientUserApplication;
        private readonly ICommonApplication _commonApplication;
        private readonly IRedis _redis;
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpContextAccessor _httpContext;

        public PatientAccountController(IPatientUserApplication patientUserApplication, ICommonApplication commonApplication, IRedis redis, IMemoryCache memoryCache,IHttpContextAccessor httpContext)
        {
            _patientUserApplication = patientUserApplication;
            _commonApplication = commonApplication;
            _redis = redis;
            _memoryCache = memoryCache;
            _httpContext = httpContext;
        }
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult LogOut()
        {
            _httpContext.HttpContext.SignOutAsync();
            return Redirect("/PatientAccount/Login");
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginInputDto input)
        {
            var result = await _patientUserApplication.LoginAsync(input);
            if (result.IsSuccess)
            {
                return RedirectToAction("DashBoard");
            }
            else
            {
                ViewBag.Error = result.Message;
                return View();
            }
        }

        public IActionResult ForgetPassWord()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Register(RegisterInputDto input)
        {

            return Json(await _patientUserApplication.RegisterAsync(input));
        }

        [Authorize]
        public IActionResult DashBoard()
        {
            return View();
        }

        [Authorize]

        public IActionResult PatientDetail()
        {
            return View();
        }

        public JsonResult SendVerification(string email)
        {
            if (email.IsNullOrWhiteSpace())
                return Json(new ModelResult { IsSuccess = false, Message = "请输入邮箱" });
            Random r = new Random();
            int num = r.Next(100000, 999999);

            var code = _memoryCache.Get(email);
            if (code != null)
                return Json(new ModelResult { IsSuccess = false, Message = "请不要重复发送验证码" });
            _memoryCache.Set(email, num, TimeSpan.FromMinutes(5));
            EmailHelper.Send(email, "重置密码", $"您正在找回你的密码，验证码为<h3>{num}</h3>");
            //var content = redis.StringGet(email);
            //if (content.HasValue)

            //redis.StringSet(email, num.ToString(), new TimeSpan(
            //    5000));
            return Json(ModelResult.Instance);
        }

        public async Task<JsonResult> ResetPassWord(ResetPassWordDto inputDto)
        {
            return Json(await _patientUserApplication.ResetPassWordAsync(inputDto));
        }
    }
}
