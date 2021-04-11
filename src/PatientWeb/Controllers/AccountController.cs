using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABPExample.Domain.Dtos.UserDtos;
using HIS.Application.Interface;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using RegisterInputDto = HIS.Domain.Dtos.PatientUser.RegisterInputDto;

namespace PatientWeb.Controllers
{
    public class AccountController : AbpController
    {
        private readonly IPatientUserApplication _patientUserApplication;

        public AccountController(IPatientUserApplication patientUserApplication)
        {
            _patientUserApplication = patientUserApplication;
        }
        public IActionResult Login()
        {
            return View();
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

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Register(RegisterInputDto input)
        {

            return Json(await _patientUserApplication.RegisterAsync(input));
        }

        public IActionResult DashBoard()
        {
            return View();
        }

        public IActionResult PatientDetail()
        {
           return View();
        }
    }
}
