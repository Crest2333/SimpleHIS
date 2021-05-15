using ABPExample.Domain.Public;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ABPExample.Domain.Models;

namespace ABPExample.Query.Interface
{
    public interface IAppointmentQuery
    {
        Task BatchUpdate(List<Appointment> entityList);

        Task<List<Appointment>> GetExpireAppointmentAsync();
    }
}
