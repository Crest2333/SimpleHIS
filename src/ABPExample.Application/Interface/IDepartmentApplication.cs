﻿using ABPExample.Domain.Dtos.Common;
using ABPExample.Domain.Dtos.Department;
using ABPExample.Domain.Dtos.Doctor;
using ABPExample.Domain.Public;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ABPExample.Domain.Dtos.Scheduling;
using HIS.Domain.Dtos.Department;
using DoctorInfoDto = ABPExample.Domain.Dtos.Doctor.DoctorInfoDto;

namespace ABPExample.Application.Interface
{
    public interface IDepartmentApplication
    {
        Task<ModelResult> Add(AddDepartmentInputDto inputDto);

        Task<ModelResult> Delete(long id);

        Task<ModelResult> Modify(int id);

        Task<ModelResult<PageDto<DepartmentInfoListDto>>> List(DepartmentSearchDto inputDto);

        Task<ModelResult> AddDepartmentPersonnel(AddPersonnelInputDto inputDto);

        Task<ModelResult> RemovePersonnel(int id);

        Task<ModelResult> BatchAdd(List<AddDepartmentInputDto> inputDtoList);

        Task<ModelResult> BatchAddDepartmentPersonnel(List<AddPersonnelInputDto> inputDtoList);

        Task<ModelResult> BatchDelete(List<int> idList);

        Task<ModelResult> BatchRemovePersonnel(List<int> idList);

        Task<ModelResult<PageDto<DoctorInfoDto>>> GetDoctorInfoAsync(GetDoctorInputDto param);

        Task<ModelResult<List<DepartmentInfoListDto>>> GetAllDepartment();

        Task<ModelResult<PageDto<SchedulingInfoDto>>> GetSchedulingInfo(GetSchedulingInputDto param);

        Task<ModelResult<bool>> AddScheduling(AddSchedulingInputDto input);

        Task<List<DepartmentInfoListDto>> GetDepartmentByDoctorIdAsync(int doctorId);

        Task<ModelResult> DeleteDeportmentDocAsync(int id);

        Task<ModelResult<List<DepartmentDoctorInfoDto>>> GetDoctorListAsync();

        Task<ModelResult<SchedulingInfoDto>> GetSchedulingByIdAsync(int id);
        Task<ModelResult> DeleteSchedulingAsync(int schedulingId);
    }
}
