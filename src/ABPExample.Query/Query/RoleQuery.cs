using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABPExample.Domain.Dtos.Role;
using ABPExample.Domain.Models;
using ABPExample.Domain.Public;
using ABPExample.EntityFramework.EntityFrameworkCore;
using ABPExample.Query.Interface;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace ABPExample.Query.Query
{
    public class RoleQuery : IRoleQuery, ITransientDependency
    {
        private readonly IAppDbContext _context;
        private readonly IObjectMapper<ABPExampleQueryModule> _mapper;

        public RoleQuery(IAppDbContext context, IObjectMapper<ABPExampleQueryModule> mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ModelResult> AddOrEditRole(AddRoleInputDto input)
        {

            var query = await _context.RoleMapper.FirstOrDefaultAsync(c =>
                c.UserId == input.UserId && c.RoleId == input.RoleId);

            if (query != null)
            {
                query.RoleId = input.RoleId;
                _context.Update(query);
                await _context.SaveChangesAsync();
                return new ModelResult {Message = "添加成功"};
            }
            else
            {
                var model = new RoleMapper
                {
                    UserId = input.UserId,
                    RoleId = input.RoleId
                };
                await _context.AddAsync(model);
                await _context.SaveChangesAsync();
                return new ModelResult {Message = "添加成功"};
            }

        }

        public async Task<ModelResult<List<RoleInfoDto>>> GetAllRole()
        {
            var query = await _context.Role.Where(c => !c.IsDeleted).ToListAsync();
            return new ModelResult<List<RoleInfoDto>> { Result = _mapper.Map<List<Role>, List<RoleInfoDto>>(query) };
        }


    }
}
