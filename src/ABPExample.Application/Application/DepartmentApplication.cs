using ABPExample.Application.Interface;
using ABPExample.Domain.Dtos.Common;
using ABPExample.Domain.Dtos.Department;
using ABPExample.Domain.Dtos.Doctor;
using ABPExample.Domain.Public;
using ABPExample.Query.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace ABPExample.Application.Application
{
    public class DepartmentApplication : IDepartmentApplication, ITransientDependency
    {
        private readonly IDepartmentQuery _departmentQuery;
        private readonly IDoctorQuery _doctorQuery;

        public DepartmentApplication(IDepartmentQuery departmentQuery,IDoctorQuery doctorQuery)
        {
            _departmentQuery = departmentQuery;
            _doctorQuery = doctorQuery;
        }
        public async Task<ModelResult> Add(AddDepartmentInputDto inputDto)
        {
            return await _departmentQuery.Add(inputDto);
        }

        public async Task<ModelResult> AddDepartmentPersonnel(AddPersonnelInputDto inputDto)
        {
            return await _departmentQuery.AddDepartmentPersonnel(inputDto);
        }

        public async Task<ModelResult> BatchAdd(List<AddDepartmentInputDto> inputDtoList)
        {
            return await _departmentQuery.BatchAdd(inputDtoList);
        }

        public async Task<ModelResult> BatchAddDepartmentPersonnel(List<AddPersonnelInputDto> inputDtoList)
        {
            return await _departmentQuery.BatchAddDepartmentPersonnel(inputDtoList);
        }

        public async Task<ModelResult> BatchDelete(List<int> idList)
        {
            return await _departmentQuery.BatchDelete(idList);
        }

        public async Task<ModelResult> BatchRemovePersonnel(List<int> idList)
        {
            return await _departmentQuery.BatchRemovePersonnel(idList);
        }

        public async Task<ModelResult> Delete(long id)
        {
            return await _departmentQuery.Delete(id);
        }

        public async Task<ModelResult<PageDto<DoctorInfoDto>>> GetDoctorInfoAsync(GetDoctorInputDto param)
        {
            return await _doctorQuery.ListAsync(param);
        }

        public async Task<ModelResult<PageDto<DepartmentInfoListDto>>> List(DepartmentSearchDto inputDto)
        {
            return new ModelResult<PageDto<DepartmentInfoListDto>> { Result = await _departmentQuery.List(inputDto), IsSuccess = true };
        }

        public async Task<ModelResult> Modify(int id)
        {
            return await _departmentQuery.Modify(id);
        }

        public async Task<ModelResult> RemovePersonnel(int id)
        {
            return await _departmentQuery.RemovePersonnel(id);
        }
    }
}
