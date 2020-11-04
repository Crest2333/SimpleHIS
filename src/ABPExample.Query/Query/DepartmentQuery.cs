﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABPExample.Domain.Dtos.Department;
using ABPExample.Domain.Models;
using ABPExample.Domain.Public;
using ABPExample.EntityFramework.EntityFrameworkCore;
using ABPExample.Query.Interface;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;

namespace ABPExample.Query.Query
{
    public class DepartmentQuery : IDepartmentQuery, ITransientDependency
    {
        private readonly IAppDbContext _context;

        public DepartmentQuery(IAppDbContext context)
        {
            _context = context;
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
                AccountNo = inputDto.AccountNo,
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
                    AccountNo = inputDto.AccountNo,
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

        public async Task<List<DepartmentInfoListDto>> List(DepartmentSearchDto inputDto)
        {
            var query = await _context.Department
                .Where(c => c.IsDeleted)
                .Skip(inputDto.PageSize * (inputDto.PageIndex - 1))
                .Take(inputDto.PageSize)
                .Select(c => new DepartmentInfoListDto
                {
                    Name=c.Name,
                    CreationTime=c.CreationTime,
                    Id=c.Id
                })
                .ToListAsync();


            return query;
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
