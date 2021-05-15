using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABPExample.Application.Interface;
using ABPExample.Domain.Dtos.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize]
    public class RoleController:AbpController
    {
        private readonly IRoleApplication _roleApplication;

        public RoleController(IRoleApplication roleApplication)
        {
            _roleApplication = roleApplication;
        }

        public async Task<JsonResult> GetAllRole()
        {
            return Json(await _roleApplication.GetAllRole());
        }

        public async Task<JsonResult> AddOrEditRole(AddRoleInputDto input)
        {
            return Json(await _roleApplication.AddOrEditRole(input));
        }

        public async Task<JsonResult> GetUserRoleByUserId(int userId)
        {
            return Json(await _roleApplication.GetUserRoleByUserIdAsync(userId));
        }
    }
}
