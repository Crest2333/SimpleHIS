﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ABPExample.Application.Common;
using ABPExample.Application.Interface;
using ABPExample.Domain.Dtos.UserDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize]

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
        public async Task<FileResult> BatchAddUser(IFormFile file)
        {
            if (!file.Name.Contains("xlsx") && !file.Name.Contains("xls"))
                return null;
            var dt = await _userApplication.BatchAddUser(file);
            var steam = ExcelHelper.Export(dt.Result);
            return File(steam, contentType: "application/x-xls", "error.xls");
        }

        public async Task<JsonResult> EditUser(EditUserInfoDto inputDto)
        {
            return Json(await _userApplication.EditUserInfo(inputDto));
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

        public async Task<JsonResult> GetUserInfoBuUserId(int userId)
        {
            return Json(await _userApplication.GetUserInfoDetail(userId));
        }

        public IActionResult UserRoleList()
        {
            return View();
        }

        public async Task<JsonResult> DeleteRole(int userId)
        {
            return Json(await _userApplication.DeleteUserRole(userId));
        }

        public async Task<JsonResult> DeleteUser(int userId)
        {
            return Json(await _userApplication.DeleteUser(userId));
        }

        public IActionResult DoctorManager()
        {
            return View();
        }
    }
}
