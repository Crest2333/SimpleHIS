using ABPExample.Domain.Dtos.Department;
using ABPExample.Domain.Public;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.MultiTenancy;

namespace ABPExample.Query.Interface
{
    public interface IDepartmentQuery
    {

        Task<ModelResult> Add( AddDepartmentInputDto inputDto);

        Task<ModelResult> Delete(long id);

        Task<ModelResult> Modify(int id);

        Task<List<DepartmentInfoListDto>> List(DepartmentSearchDto inputDto);

        Task<ModelResult> AddDepartmentPersonnel(AddPersonnelInputDto inputDto);

        Task<ModelResult> RemovePersonnel(int id);

        Task<ModelResult> BatchAdd(List<AddDepartmentInputDto> inputDtoList);

        Task<ModelResult> BatchAddDepartmentPersonnel(List<AddPersonnelInputDto> inputDtoList);

        Task<ModelResult> BatchDelete(List<int> idList);

        Task<ModelResult> BatchRemovePersonnel(List<int> idList);

       
    }
}
