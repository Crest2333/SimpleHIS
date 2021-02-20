using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ABPExample.Domain.Dtos.Common;
using ABPExample.Domain.Dtos.Doctor;
using ABPExample.Domain.Public;
using ABPExample.EntityFramework.EntityFrameworkCore;
using ABPExample.Query.Interface;
using Volo.Abp.DependencyInjection;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ABPExample.Query.Query
{
    public class DoctorQuery : IDoctorQuery, ITransientDependency
    {
        private readonly IAppDbContext _context;

        public DoctorQuery(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<ModelResult<DoctorInfoDto>> DetailAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ModelResult<PageDto<DoctorInfoDto>>> ListAsync(GetDoctorInputDto param)
        {
            var query = from a in _context.Users
                        from b in _context.DepartmentMapper.Where(c => c.UserId == a.Id)
                        from d in _context.Department.Where(c => c.Id == b.DepartmentId)
                        where string.IsNullOrEmpty(param.Name) || a.UserName == param.Name
                        where param.DepartmentId <= 0 || param.DepartmentId == d.Id
                        orderby a.Id descending
                        select new DoctorInfoDto
                        {
                            WorkStartDate = b.CreationTime,
                            DepartmentId = d.Id,
                            DepartmentName = d.Name,
                            Gender = a.Gender == Domain.Models.Gender.Man ? "男" : a.Gender == Domain.Models.Gender.Woman ? "女" : "其他",
                            Id = a.Id,
                            Name = a.UserName,
                            WorkNo = a.UserAccount
                        };
            var count = await query.CountAsync();
            var list =await query.Skip((param.PageIndex - 1) * param.PageSize).Take(param.PageSize).ToListAsync();
            return new ModelResult<PageDto<DoctorInfoDto>> { IsSuccess = true, Result = new PageDto<DoctorInfoDto>(count, list) };
        }
    }
}
