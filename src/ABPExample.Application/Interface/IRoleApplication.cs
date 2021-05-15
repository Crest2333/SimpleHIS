using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ABPExample.Domain.Dtos.Role;
using ABPExample.Domain.Public;

namespace ABPExample.Application.Interface
{
    public interface IRoleApplication
    {
        Task<ModelResult> AddOrEditRole(AddRoleInputDto input);

        Task<ModelResult<List<RoleInfoDto>>> GetAllRole();

        Task<ModelResult<RoleInfoDto>> GetUserRoleByUserIdAsync(int userId);
    }
}
