using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ABPExample.Application.Interface;
using ABPExample.Domain.Dtos.Role;
using ABPExample.Domain.Public;
using ABPExample.Query.Interface;
using Volo.Abp.DependencyInjection;

namespace ABPExample.Application.Application
{
    public class RoleApplication: IRoleApplication,ITransientDependency
    {
        private readonly IRoleQuery _roleQuery;

        public RoleApplication(IRoleQuery roleQuery)
        {
            _roleQuery = roleQuery;
        }

        public async Task<ModelResult> AddOrEditRole(AddRoleInputDto input)
        {
            return await _roleQuery.AddOrEditRole(input);
        }

        public async Task<ModelResult<List<RoleInfoDto>>> GetAllRole()
        {
            return await _roleQuery.GetAllRole();
        }

        public async Task<ModelResult<RoleInfoDto>> GetUserRoleByUserIdAsync(int userId)
        {
            return await _roleQuery.GetUserRoleByUserIdAsync(userId);
        }
    }
}
