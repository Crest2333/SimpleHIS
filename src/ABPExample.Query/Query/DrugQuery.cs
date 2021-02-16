using ABPExample.Domain.Dtos.Common;
using ABPExample.Domain.Dtos.Drug;
using ABPExample.Domain.Models;
using ABPExample.Domain.Public;
using ABPExample.EntityFramework.EntityFrameworkCore;
using ABPExample.Query.Interface;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace ABPExample.Query.Query
{
    public class DrugQuery : IDrugQuery, ITransientDependency
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public DrugQuery(IAppDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ModelResult> Add(AddDrugInputDto inputDto)
        {
            var query = _mapper.Map<Drug>(inputDto);
            query.CreationTime = DateTime.Now;
            query.LastModificationTime = DateTime.Now;
            query.IsDeleted = false;
            await _context.AddAsync(query);
            await _context.SaveChangesAsync();
            return new ModelResult { IsSuccess = true, Message = "添加成功" };
        }

        public async Task<ModelResult> BatchAdd(List<AddDrugInputDto> inputDtoList)
        {
            var query = new List<Drug>();
            query = _mapper.Map<List<Drug>>(inputDtoList);
            query.ForEach(item =>
            {
                item.LastModificationTime = DateTime.Now;
                item.CreationTime = DateTime.Now;
                item.IsDeleted = true;
            });

            await _context.AddRangeAsync(query);
            await _context.SaveChangesAsync();
            return new ModelResult { IsSuccess = true, Message = "添加成功" };
        }

        public async Task<ModelResult> BatchAddCompatibility(int inDrugId, List<int> onDrugIdList)
        {
            var existenceList = await _context.DrugCompatibility
                .Where(c => (c.InDrugId == inDrugId && onDrugIdList.Contains(c.OnDrugId) || (c.OnDrugId == inDrugId && onDrugIdList.Contains(c.InDrugId))))
                .ToListAsync();
            if (existenceList.Any())
            {
                var idList = new List<int>();
                idList.AddRange(existenceList.Select(c => c.InDrugId));
                idList.AddRange(existenceList.Select(c => c.OnDrugId));
                var disIdList = idList.Distinct().ToList();
                onDrugIdList = onDrugIdList.Except(disIdList).ToList();
            }
            var query = new List<DrugCompatibility>();
            onDrugIdList.ForEach(item =>
            {
                var model = new DrugCompatibility
                {
                    InDrugId = inDrugId,
                    OnDrugId = item,
                    CreationTime = DateTime.Now,
                    IsDeleted = false,
                    LastModificationTime = DateTime.Now
                };
                query.Add(model);
            });

            await _context.AddRangeAsync(query);
            await _context.SaveChangesAsync();
            return new ModelResult { IsSuccess = true, Message = "添加成功" };
        }

        public async Task<ModelResult> BatchDelete(List<int> idList)
        {
            var query = await _context.Drug.Where(c => idList.Contains(c.Id) && !c.IsDeleted).ToListAsync();
            query.ForEach(item =>
            {
                item.IsDeleted = true;
                item.LastModificationTime = DateTime.Now;
            });
            return new ModelResult { IsSuccess = true, Message = "删除成功" };
        }

        public async Task<ModelResult> BatchDeleteCompatibility(List<int> idList)
        {
            var query = await _context.DrugCompatibility.Where(c => idList.Contains(c.Id) && !c.IsDeleted).ToListAsync();
            if (!query.Any())
                return new ModelResult { IsSuccess = false, Message = "没找到相关数据" };

            query.ForEach(item =>
            {
                item.IsDeleted = true;
                item.LastModificationTime = DateTime.Now;
            });
            _context.UpdateRange(query);
            await _context.SaveChangesAsync();
            return new ModelResult { IsSuccess = true, Message = "删除成功" };
        }

        public async Task<ModelResult> Delete(int id)
        {
            var query = await _context.Drug.FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
            if (query == null)
                return new ModelResult { IsSuccess = false, Message = "没有找到相关信息" };
            query.IsDeleted = true;
            query.LastModificationTime = DateTime.Now;
            return new ModelResult { IsSuccess = true, Message = "删除成功" };
        }

        public async Task<ModelResult> Edit(EditDrugInputDto inputDto)
        {
            var query = await _context.Drug.FirstOrDefaultAsync(c => c.Id == inputDto.Id && !c.IsDeleted);
            if (query == null)
                return new ModelResult { IsSuccess = false, Message = "没有找到相关信息" };
            query = _mapper.Map<Drug>(inputDto);
            query.LastModificationTime = DateTime.Now;
            _context.Update(query);
            await _context.SaveChangesAsync();

            return new ModelResult { IsSuccess = true, Message = "修改成功" };
        }

        public async Task<ModelResult<PageDto<DrugInfoListDto>>> List(DrugInfoSearchDto param)
        {
            var query = _context.Drug
                .WhereIf(string.IsNullOrEmpty(param.Name), c => c.Name.Contains(param.Name) && !c.IsDeleted);
            var count = await query.CountAsync();
            var list = await query.Skip((param.PageIndex - 1) * param.PageSize).Take(param.PageSize).ToListAsync();

            var result = new List<DrugInfoListDto>();
            result = _mapper.Map<List<DrugInfoListDto>>(list);
            return new ModelResult<PageDto<DrugInfoListDto>> { IsSuccess = true, Result = new PageDto<DrugInfoListDto>(count, result) };
        }

        public Task<ModelResult<bool>> Verify(int coverId, int id)
        {
            throw new NotImplementedException();
        }
    }
}
