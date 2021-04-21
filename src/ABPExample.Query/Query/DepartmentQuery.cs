using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABPExample.Domain.Dtos.Common;
using ABPExample.Domain.Dtos.Department;
using ABPExample.Domain.Dtos.Scheduling;
using ABPExample.Domain.Models;
using ABPExample.Domain.Public;
using ABPExample.EntityFramework.EntityFrameworkCore;
using ABPExample.Query.Interface;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace ABPExample.Query.Query
{
    public class DepartmentQuery : IDepartmentQuery, ITransientDependency
    {
        private readonly IAppDbContext _context;
        private readonly IObjectMapper _mapper;
        private readonly IUserQuery _userQuery;

        public DepartmentQuery(IAppDbContext context, IObjectMapper mapper, IUserQuery userQuery)
        {
            _context = context;
            _mapper = mapper;
            _userQuery = userQuery;
        }

        public async Task<ModelResult> Add(AddDepartmentInputDto inputDto)
        {
            var model = new Department
            {
                CreationTime = DateTime.Now,
                Describe = inputDto.Describe,
                Img = inputDto.ImgUrl,
                IsDeleted = false,
                LastModificationTime = DateTime.Now,
                Name = inputDto.Name
            };
            await _context.AddAsync(model);
            await _context.SaveChangesAsync();
            return new ModelResult { IsSuccess = true, Message = "添加成功！" };
        }

        public async Task<ModelResult> AddDepartmentPersonnel(AddPersonnelInputDto inputDto)
        {
            var model = new DepartmentMapper
            {
                CreationTime = DateTime.Now,
                DepartmentId = inputDto.DepartmentId,
                UserId = inputDto.UserId,
                IsDeleted = false,
            };
            await _context.AddAsync(model);
            await _context.SaveChangesAsync();
            return new ModelResult { IsSuccess = true, Message = "添加成功！" };
        }

        public async Task<ModelResult> BatchAdd(List<AddDepartmentInputDto> inputDtoList)
        {
            var modelList = new List<Department>();
            inputDtoList.ForEach(inputDto =>
            {
                var model = new Department
                {
                    CreationTime = DateTime.Now,
                    Describe = inputDto.Describe,
                    Img = inputDto.ImgUrl,
                    IsDeleted = false,
                    LastModificationTime = DateTime.Now,
                    Name = inputDto.Name
                };
                modelList.Add(model);
            });
            await _context.AddRangeAsync(modelList);
            await _context.SaveChangesAsync();
            return new ModelResult { IsSuccess = true, Message = "添加成功！" };
        }

        public async Task<ModelResult> BatchAddDepartmentPersonnel(List<AddPersonnelInputDto> inputDtoList)
        {
            var modelList = new List<DepartmentMapper>();
            inputDtoList.ForEach(inputDto =>
            {
                var model = new DepartmentMapper
                {
                    CreationTime = DateTime.Now,
                    DepartmentId = inputDto.DepartmentId,
                    UserId = inputDto.UserId,
                    IsDeleted = false,
                };
                modelList.Add(model);
            });
            await _context.AddRangeAsync(modelList);
            await _context.SaveChangesAsync();
            return new ModelResult { IsSuccess = true, Message = "添加成功！" };
        }

        public async Task<ModelResult> BatchDelete(List<int> idList)
        {
            var query = await _context.Department.Where(c => idList.Contains((int)c.Id) && !c.IsDeleted).ToListAsync();

            query.ForEach(item =>
            {
                item.IsDeleted = true;
            });
            _context.UpdateRange(query);
            await _context.SaveChangesAsync();
            return new ModelResult { IsSuccess = true, Message = "删除成功！" };

        }

        public async Task<ModelResult> BatchRemovePersonnel(List<int> idList)
        {
            var query = await _context.DepartmentMapper.Where(c => idList.Contains(c.Id) && !c.IsDeleted).ToListAsync();

            query.ForEach(item =>
            {
                item.IsDeleted = true;
            });
            _context.UpdateRange(query);
            await _context.SaveChangesAsync();
            return new ModelResult { IsSuccess = true, Message = "删除成功！" };
        }

        public async Task<ModelResult<List<DepartmentInfoListDto>>> GetAllDepartment()
        {
            var result = await _context.Department.Where(c => !c.IsDeleted).ToListAsync();
            return new ModelResult<List<DepartmentInfoListDto>>
            { IsSuccess = true, Result = _mapper.Map<List<Department>, List<DepartmentInfoListDto>>(result) };
        }

        public async Task<ModelResult<PageDto<SchedulingInfoDto>>> GetSchedulingInfo(GetSchedulingInputDto param)
        {
            var query = from a in _context.Scheduling
                        join b in _context.Users on a.UserId equals b.Id
                        join department in _context.Department on a.DepartmentId equals department.Id
                        join c in _context.Users on a.OprId equals c.Id into d
                        from e in d.DefaultIfEmpty()
                        where !a.IsDeleted
                        where string.IsNullOrEmpty(param.Name) || param.Name == b.UserName
                        where string.IsNullOrEmpty(param.UseNo) || param.UseNo == b.UserAccount
                        where !param.DepartmentId.HasValue || param.DepartmentId == a.DepartmentId
                        where !param.StartDate.HasValue || a.StartDate >= param.StartDate
                        where !param.EndDate.HasValue || a.EndDate <= param.EndDate
                        select new SchedulingInfoDto
                        {
                            DepartmentName = department.Name,
                            SchedulingType = a.SchedulingType,
                            StartDate = a.StartDate,
                            EndDate = a.EndDate,
                            UserId = a.UserId,
                            UserName = b.UserName,
                            OprDate = a.OprDate,
                            OprName = e.UserName,
                            OprNo = e.UserAccount,
                            UserNo = a.UserNo,
                            Id = a.Id
                        };
            var count = await query.CountAsync();
            var list = await query
                .OrderBy(c => c.StartDate)
                .Skip(param.PageSize * (param.PageIndex - 1))
                .Take(param.PageSize)
                .ToListAsync();

            return new ModelResult<PageDto<SchedulingInfoDto>> { Result = new PageDto<SchedulingInfoDto>(count, list) };
        }

        public async Task<ModelResult<bool>> AddScheduling(AddSchedulingInputDto input)
        {
            var model = _mapper.Map<AddSchedulingInputDto, Scheduling>(input);
            var userInfo = await _userQuery.GetUserInfoAsync(new List<string> { input.UserNo });
            if (!userInfo.Any())
                return new ModelResult<bool> { IsSuccess = false, Message = "无效用户ID" };
            var schedulingList = await _context.Scheduling
                .Where(c => c.UserNo == input.UserNo && !c.IsDeleted)
                .Where(c => c.StartDate <= input.EndDate && c.EndDate >= input.StartDate)
                .OrderBy(c => c.StartDate)
                .ToListAsync();

            if (schedulingList.Any())
            {
                var info = schedulingList
                    .FirstOrDefault(c => c.StartDate < input.StartDate && c.EndDate <= input.EndDate);
                if (info != null)
                {
                    info.EndDate = input.StartDate;
                    _context.Modified(info, c => c.EndDate);
                }

                var middleInfo = schedulingList.Where(c => c.StartDate > input.StartDate && c.EndDate < input.EndDate)
                    .ToList();

                middleInfo.ForEach(item =>
                {
                    item.IsDeleted = true;
                    _context.Modified(item, c => c.IsDeleted);
                });

                var lastInfo = schedulingList
                    .FirstOrDefault(c => c.StartDate < input.EndDate && c.EndDate > input.EndDate);
                if (lastInfo != null)
                {
                    lastInfo.StartDate = input.EndDate;
                    _context.Modified(info, c => c.StartDate);
                }
            }

            model.OprDate = DateTime.Now;
            model.OprNo = "SYS";
            model.OprId = -1;
            model.UserId = userInfo.First().UserId;
            await _context.AddAsync(model);
            await _context.SaveChangesAsync();
            return new ModelResult<bool> { Message = "添加成功", Result = true };
        }

        public async Task<List<DepartmentInfoListDto>> GetDepartmentByDoctorIdAsync(int doctorId)
        {
            return await (from a in _context.DepartmentMapper
                          join b in _context.Department on a.DepartmentId equals b.Id
                          where doctorId == a.UserId && !a.IsDeleted
                          select new DepartmentInfoListDto
                          {
                              Id = b.Id,
                              Name = b.Name
                          }).ToListAsync();
        }

        public async Task<ModelResult> Delete(long id)
        {
            var query = await _context.Department.Where(c => c.Id == id && !c.IsDeleted).FirstOrDefaultAsync();

            if (query == null)
                return new ModelResult { IsSuccess = false, Message = "没有找到相关信息！" };
            query.IsDeleted = true;
            _context.UpdateRange(query);
            await _context.SaveChangesAsync();
            return new ModelResult { IsSuccess = true, Message = "删除成功！" };
        }

        public async Task<PageDto<DepartmentInfoListDto>> List(DepartmentSearchDto inputDto)
        {
            var query =
                _context.Department
                .WhereIf(!string.IsNullOrWhiteSpace(inputDto.Name), c => c.Name == inputDto.Name)
                .Where(c => !c.IsDeleted);
            var count = await query.CountAsync();
            var list = await query
                .Skip(inputDto.PageSize * (inputDto.PageIndex - 1))
                .Take(inputDto.PageSize)
                .Select(c => new DepartmentInfoListDto
                {
                    Name = c.Name,
                    CreationTime = c.CreationTime,
                    Id = c.Id
                })
                .ToListAsync();

            var departmentIdList = list.Select(c => c.Id).ToList();
            var userCount = await (
                from a in _context.DepartmentMapper
                join b in _context.Users on a.UserId equals b.Id
                where !a.IsDeleted && !b.IsDeleted && departmentIdList.Contains(a.DepartmentId)
                group a by a.DepartmentId
                into e
                select new
                {
                    e.Key,
                    TotalCount = e.Count()
                }).ToListAsync();

            foreach (var item in list)
            {
                item.PersonnelCount = userCount.FirstOrDefault(c => c.Key == item.Id)?.TotalCount ?? 0;
            }
            return new PageDto<DepartmentInfoListDto>(count, list);
        }

        public async Task<ModelResult> Modify(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ModelResult> RemovePersonnel(int id)
        {
            var query = await _context.DepartmentMapper.Where(c => c.Id == id && !c.IsDeleted).FirstOrDefaultAsync();

            if (query == null)
                return new ModelResult { IsSuccess = false, Message = "没有找到相关信息！" };
            query.IsDeleted = true;
            _context.UpdateRange(query);
            await _context.SaveChangesAsync();
            return new ModelResult { IsSuccess = true, Message = "删除成功！" };
        }
    }
}
