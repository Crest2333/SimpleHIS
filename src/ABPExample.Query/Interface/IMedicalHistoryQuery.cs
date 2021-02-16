using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ABPExample.Domain.Dtos.Common;
using ABPExample.Domain.Dtos.MedicalHistory;
using ABPExample.Domain.Public;

namespace ABPExample.Query.Interface
{
    public interface IMedicalHistoryQuery
    {
        Task<ModelResult<PageDto<MedicalInfoDto>>> List(GetMedicalInputDto param);

        Task<ModelResult> Add(AddPastHistoryDto input);

        Task<ModelResult> Delete(int id );

        Task<ModelResult> Edit(EditMedicalInputDto input);
    }
}
