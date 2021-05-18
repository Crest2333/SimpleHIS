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
using ABPExample.Domain.Dtos.Scheduling;
using HIS.Domain.Dtos.Department;
using Volo.Abp.DependencyInjection;
using DoctorInfoDto = ABPExample.Domain.Dtos.Doctor.DoctorInfoDto;

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
            if (inputDto.Name.IsNullOrWhiteSpace())
                return new ModelResult {IsSuccess = false, Message = "请输入名称"};

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

        public async Task<ModelResult<List<DepartmentInfoListDto>>> GetAllDepartment()
        {
            return await _departmentQuery.GetAllDepartment();
        }

        public async Task<ModelResult<PageDto<SchedulingInfoDto>>> GetSchedulingInfo(GetSchedulingInputDto param)
        {
            return await _departmentQuery.GetSchedulingInfo(param);

        }

        public async Task<ModelResult<bool>> AddScheduling(AddSchedulingInputDto input)
        {
            return await _departmentQuery.AddScheduling(input);
        }

        public async Task<List<DepartmentInfoListDto>> GetDepartmentByDoctorIdAsync(int doctorId)
        {
           return await _departmentQuery.GetDepartmentByDoctorIdAsync(doctorId);
        }

        public Task<ModelResult> DeleteDeportmentDocAsync(int id)
        {
            return _departmentQuery.DeleteDeportmentDocAsync(id);
        }

        public Task<ModelResult<List<DepartmentDoctorInfoDto>>> GetDoctorListAsync()
        {
            return _departmentQuery.GetDoctorListAsync();
        }

        public async Task<ModelResult<SchedulingInfoDto>> GetSchedulingByIdAsync(int id)
        {
            return await _departmentQuery.GetSchedulingByIdAsync(id);
        }

        public async Task<ModelResult> DeleteSchedulingAsync(int schedulingId)
        {
            return await _departmentQuery.DeleteSchedulingAsync(schedulingId);
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
