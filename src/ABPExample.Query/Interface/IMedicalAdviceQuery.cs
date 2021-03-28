using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ABPExample.Domain.Dtos.MedicalAdvice;
using ABPExample.Domain.Models;
using ABPExample.Domain.Public;

namespace ABPExample.Query.Interface
{
    public interface IMedicalAdviceQuery
    {
        Task<ModelResult<MedicalAdviceEntityDto>> GetMedicalAdviceAsync(int appointmentId);

        Task<ModelResult> AddMedicalAdviceAsync(MedicalAdviceInputDto input);

        Task<ModelResult> EditMedicalAdviceAsync(MedicalAdviceInputDto input);

    }
}
