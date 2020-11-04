using ABPExample.Domain.Public;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ABPExample.Query.Interface
{
    public interface IAppointmentQuery
    {
        Task<ModelResult> Add();

        Task<ModelResult> Delete();

        Task<ModelResult> Edit();

        Task<ModelResult> List();
    }
}
