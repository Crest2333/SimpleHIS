using ABPExample.Domain.Dtos.Common;
using ABPExample.Domain.Dtos.Department;
using ABPExample.Domain.Public;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ABPExample.Domain.Dtos.Scheduling;
using Volo.Abp.MultiTenancy;

namespace ABPExample.Query.Interface
{
    public interface IDepartmentQuery
    {

        Task<ModelResult> Add( AddDepartmentInputDto inputDto);

        Task<ModelResult> Delete(long id);

        Task<ModelResult> Modify(int id);

        Task<PageDto<DepartmentInfoListDto>> List(DepartmentSearchDto inputDto);

        Task<ModelResult> AddDepartmentPersonnel(AddPersonnelInputDto inputDto);

        Task<ModelResult> RemovePersonnel(int id);

        Task<ModelResult> BatchAdd(List<AddDepartmentInputDto> inputDtoList);

        Task<ModelResult> BatchAddDepartmentPersonnel(List<AddPersonnelInputDto> inputDtoList);

        Task<ModelResult> BatchDelete(List<int> idList);

        Task<ModelResult> BatchRemovePersonnel(List<int> idList);

        Task<ModelResult<List<DepartmentInfoListDto>>> GetAllDepartment();
       
        Task<ModelResult<PageDto<SchedulingInfoDto>>> GetSchedulingInfo(GetSchedulingInputDto getSchedulingInputDto);

        Task<ModelResult<bool>> AddScheduling(AddSchedulingInputDto input);
    }
}
