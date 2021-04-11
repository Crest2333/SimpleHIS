using ABPExample.Domain.Dtos.Common;
using ABPExample.Domain.Dtos.Patient;
using ABPExample.Domain.Models;
using ABPExample.Domain.Public;
using ABPExample.EntityFramework.EntityFrameworkCore;
using ABPExample.Query.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ABPExample.Domain.Dtos.MedicalHistory;
using Microsoft.EntityFrameworkCore.Internal;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;


namespace ABPExample.Query.Query
{
   public class PatientQuery : IPatientQuery, ITransientDependency
   {
      private readonly IAppDbContext _context;
      private readonly IObjectMapper<ABPExampleQueryModule> _mapper;
      private readonly IQueryProvider _queryProvider;

      public PatientQuery(IAppDbContext context, IObjectMapper<ABPExampleQueryModule> mapper,IQueryProvider queryProvider)
      {
         _context = context;
         _mapper = mapper;
         _queryProvider = queryProvider;
      }
      public async Task<ModelResult<int>> Add(AddPatientInfoDto model)
      {
         var query = new Patients();
         query = _mapper.Map<AddPatientInfoDto, Patients>(model);
         query.IsDeleted = false;
         query.CreationTime = DateTime.Now;
         query.LastModificationTime = DateTime.Now;

         var isExistence = await _context.Patients
            .Where(c => c.FullName == model.FullName && c.IdentityId == model.IdentityId && !c.IsDeleted).AnyAsync();
         if (isExistence)
            return new ModelResult<int> { IsSuccess = false, Message = "添加失败，该患者已存在！" };

         await _context.AddAsync(query);
         var result = await _context.SaveChangesAsync();
         return new ModelResult<int> { Code = 200, IsSuccess = true, Message = "添加成功", Result = result };
      }

      public async Task<ModelResult> AddIllnessHistory(AddPastHistoryDto model)
      {
         var query = _mapper.Map<AddPastHistoryDto, PastHistories>(model);
         query.CreationTime = DateTime.Now;
         query.LastModificationTime = DateTime.Now;
         query.IsDeleted = false;
         query.CreateBy = "123";
         var isExistence = await _context.PastHistories
            .Where(c => !c.IsDeleted && c.Name == model.Name && c.PatientId == model.PatientId).AnyAsync();
         if (isExistence)
            return new ModelResult { IsSuccess = false, Message = "该患者的病史已有该条数据！" };

         await _context.AddAsync(query);
         await _context.SaveChangesAsync();
         return new ModelResult { Code = 200, IsSuccess = true, Message = "添加成功" };
      }

      public async Task<PatientInfoDetailDto> GetPatientDetailAsync(string identityId)
      {
         var entity = await _context.Patients.FirstOrDefaultAsync(c => c.IdentityId == identityId && !c.IsDeleted);
         return _mapper.Map<Patients, PatientInfoDetailDto>(entity);
      }

      public async Task<ModelResult> BatchAdd(List<AddPatientInfoDto> modelList)
      {

         //var query = new List<Patients>();
         //query = ObjectMapper.Map(modelList, query);
         //var existencePatient = new List<Patients>();

         //var identityList = modelList.Select(c => c.IdentityId).ToList();
         //var existenceList = await _context.Patients.Where(c => !c.IsDeleted && identityList.Contains(c.IdentityId)).ToListAsync();

         //query.ForEach(item =>
         //{
         //    item.CreationTime = DateTime.Now;
         //    item.LastModificationTime = DateTime.Now;
         //    item.IsDeleted = false;
         //});

         throw new NotImplementedException();
      }

      public async Task<ModelResult> BatchAddIllnessHistory(List<AddPastHistoryDto> modelList)
      {
         throw new NotImplementedException();
      }

      public async Task<PatientInfoDetailDto> GetPatientByUserIdAsync(int userId)
      {
         var entity = await (
            from a in _context.Patients
            join b in _context.PatientsMapping on a.Id equals b.PatientId
            where b.UserId == userId && !a.IsDeleted && !b.IsDeleted
            select a
         ).FirstOrDefaultAsync();

         return _mapper.Map<Patients, PatientInfoDetailDto>(entity);
      }

      public async Task<ModelResult> BatchDelete(List<long> id)
      {
         var query = await _context.Patients.Where(c => !c.IsDeleted && id.Contains(c.Id)).ToListAsync();
         query.ForEach(item =>
         {
            item.IsDeleted = true;
            item.LastModificationTime = DateTime.Now;

            _context.Attach(item);
            _context.Entry(item).Property(c => c.IsDeleted).IsModified = true;
            _context.Entry(item).Property(c => c.LastModificationTime).IsModified = true;
         });

         await _context.SaveChangesAsync();
         return new ModelResult { IsSuccess = true, Message = "批量删除成功！" };
      }

      public async Task<ModelResult> Delete(int id)
      {
         var query = await _context.Patients.FirstOrDefaultAsync(c => !c.IsDeleted && c.Id == id);
         if (query == null)
            return new ModelResult { IsSuccess = false, Message = "没有找到信息" };
         query.IsDeleted = true;
         query.LastModificationTime = DateTime.Now;

         _context.Update(query);
         await _context.SaveChangesAsync();
         return new ModelResult { IsSuccess = false, Message = "删除成功！" };
      }

      public async Task<ModelResult<PatientInfoDetailDto>> Detail(int id)
      {
         var info = await _context.Patients.FirstOrDefaultAsync(c => c.Id == id);
         return info == null ? new ModelResult<PatientInfoDetailDto> { IsSuccess = false, Message = "没有获取到数据" }
             : new ModelResult<PatientInfoDetailDto> { IsSuccess = true, Result = _mapper.Map<Patients, PatientInfoDetailDto>(info) };
      }

      public async Task<ModelResult> Edit(EditPatientInfoDto model)
      {
         var info = await _context.Patients.FirstOrDefaultAsync(c => c.Id == model.Id && model.IdentityId == c.IdentityId && !c.IsDeleted);
         if (info == null)
            return new ModelResult { IsSuccess = false, Message = "信息无效" };
         info.LastModificationTime = DateTime.Now;
         info.PhoneNumber = model.PhoneNumber;
         info.Weight = model.Weight;
         info.Height = model.Height;
         info.Gender = model.Gender;
         info.BloodType = model.BloodType;
         info.FullName = model.FullName;
         info.Address = model.Address;
         _context.Update(info);
         await _context.SaveChangesAsync();
         return new ModelResult { IsSuccess = true, Message = "修改成功" };
      }

      public async Task<ModelResult<PageDto<PatientInfoListDto>>> List(PatientSearchDto param)
      {
         var query = _context.Patients
             .WhereIf(!string.IsNullOrEmpty(param.Name), c => c.FullName == param.Name)
             .WhereIf(!string.IsNullOrEmpty(param.IdentityId), c => c.IdentityId == param.IdentityId)
             .WhereIf(!string.IsNullOrEmpty(param.PhoneNumber), c => c.PhoneNumber == param.PhoneNumber)
             .Where(c => !c.IsDeleted);

         var count = await query.CountAsync();
         if (param.PageIndex > 0)
            query = query.Skip((param.PageIndex - 1) * param.PageSize).Take(param.PageSize);

         var list = await query.ToListAsync();
         var result = _mapper.Map<List<Patients>, List<PatientInfoListDto>>(list);


         return new ModelResult<PageDto<PatientInfoListDto>> { IsSuccess = true, Result = new PageDto<PatientInfoListDto>(count, result) };
      }
   }
}
