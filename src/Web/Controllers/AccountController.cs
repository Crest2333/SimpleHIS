using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABPExample.Application.Interface;
using ABPExample.Domain.Dtos.UserDtos;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class AccountController : AbpController
    {
        private readonly IAccountApplication _application;

        public AccountController(IAccountApplication application)
        {
            _application = application;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginInputDto inputDto)
        {
            var result = await _application.Authentication(inputDto);
            if (result.Result)
            {
                return RedirectToAction("DashBoard");
            }
            else
            {
                ViewBag.Error = result.Message;
                return View();
            }
        }

        public IActionResult LogOut()
        {
            return View();
        }

        public IActionResult DashBoard()
        {
            return View();
        }
    }
}
