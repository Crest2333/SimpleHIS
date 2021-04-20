using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABPExample.Domain.Dtos.Common;
using ABPExample.Domain.Dtos.MedicalHistory;
using ABPExample.Domain.Models;
using ABPExample.Domain.Public;
using ABPExample.EntityFramework.EntityFrameworkCore;
using ABPExample.Query.Interface;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace ABPExample.Query.Query
{
    public class MedicalHistoryQuery : IMedicalHistoryQuery, ITransientDependency
    {
        private readonly IAppDbContext _context;
        private readonly IObjectMapper<HISQueryModule> _mapper;

        public MedicalHistoryQuery(IAppDbContext context, IObjectMapper<HISQueryModule> mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ModelResult<PageDto<MedicalInfoDto>>> List(GetMedicalInputDto param)
        {
            var query = _context.PastHistories.Where(c => c.PatientId == param.PatientId);
            var list = await query.Skip((param.PageIndex - 1) * param.PageSize).Take(param.PageSize)
                .ToListAsync();
            return new ModelResult<PageDto<MedicalInfoDto>>
            {
                IsSuccess = true,
                Result = new PageDto<MedicalInfoDto>(await query.CountAsync(), _mapper.Map<List<PastHistories>, List<MedicalInfoDto>>(list))
            };
        }

        public Task<ModelResult> Add(AddPastHistoryDto input)
        {
            throw new NotImplementedException();
        }

        public Task<ModelResult> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ModelResult> Edit(EditMedicalInputDto input)
        {
            var model = _mapper.Map<EditMedicalInputDto, PastHistories>(input);
            model.CreateBy = "123";
            model.CreationTime=DateTime.Now;
            model.LastModificationTime = DateTime.Now;

            _context.Update(model);
            await _context.SaveChangesAsync();
            return new ModelResult { IsSuccess = true, Message = "修改成功" };
        }

        public async Task<ModelResult<MedicalInfoDto>> Detail(int id)
        {
            var model = await _context.PastHistories.FirstOrDefaultAsync(c => c.Id == id);
            return model == null
                ? new ModelResult<MedicalInfoDto> { IsSuccess = false, Message = "没有查询到相关数据" }
                : new ModelResult<MedicalInfoDto>
                { IsSuccess = true, Result = _mapper.Map<PastHistories, MedicalInfoDto>(model) };
        }
    }
}
