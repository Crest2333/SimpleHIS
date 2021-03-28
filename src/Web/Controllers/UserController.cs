﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ABPExample.Application.Interface;
using ABPExample.Domain.Dtos.UserDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class UserController : AbpController
    {
        private readonly IUserApplication _userApplication;

        public UserController(IUserApplication userApplication)
        {
            _userApplication = userApplication;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult List()
        {
            return View();
        }

        
        [HttpPost]
        public async Task<JsonResult> GetUserInfoList(UserInfoListSearchDto inputDto)
        {
            return Json(await _userApplication.GetUserInfoList(inputDto));
        }

        [HttpPost]
        public async Task<JsonResult> AddUser(AddUserInputDto inputDto)
        {
            return Json(await _userApplication.AddUser(inputDto));
        }

        [HttpPost]
        public async Task<JsonResult> BatchAddUser(IFormFile file)
        {
            return Json(await _userApplication.BatchAddUser(file));
        }

        [HttpPut]
        public async Task<JsonResult> ResetPassWord(int userId)
        {
            return Json(await _userApplication.ResetUserPassWord(userId));
        }

        [HttpPost]
        public async Task<IActionResult> ExportUserInfo()
        {
            MemoryStream stream = (MemoryStream)await _userApplication.ExportUserInfo();
            return File(stream.ToArray(), "application/ms-excel", "fileName.xlsx");
        }
    }
}
