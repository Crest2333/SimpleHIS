using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ABPExample.Domain.Dtos.Role;
using ABPExample.Domain.Public;

namespace ABPExample.Query.Interface
{
    public interface IRoleQuery
    {
        Task<ModelResult> AddOrEditRole(AddRoleInputDto input);

        Task<ModelResult<List<RoleInfoDto>>> GetAllRole();
        Task<ModelResult<RoleInfoDto>> GetUserRoleByUserIdAsync(int userId);
    } 
}
