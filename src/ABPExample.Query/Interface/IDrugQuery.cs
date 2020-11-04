using ABPExample.Domain.Dtos.Common;
using ABPExample.Domain.Dtos.Drug;
using ABPExample.Domain.Public;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ABPExample.Query.Interface
{
    public interface IDrugQuery
    {
        Task<ModelResult> Add(AddDrugInputDto inputDto);

        Task<ModelResult> Delete(int id);

        Task<ModelResult> Edit(EditDrugInputDto inputDto);

        Task<ModelResult<PageDto<DrugInfoListDto>>> List(DrugInfoSearchDto param);

        Task<ModelResult<bool>> Verify(int coverId, int id);

        Task<ModelResult> BatchDelete(List<int> idList);

        Task<ModelResult> BatchAdd(List<AddDrugInputDto> inputDtoList);

        /// <summary>
        /// 批量添加配伍
        /// </summary>
        /// <returns></returns>
        Task<ModelResult> BatchAddCompatibility(int inDrugId, List<int> onDrugIdList);

        /// <summary>
        /// 批量删除配伍
        /// </summary>
        /// <returns></returns>
        Task<ModelResult> BatchDeleteCompatibility(List<int> idList);
    }
}
