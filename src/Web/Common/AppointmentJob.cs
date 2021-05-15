using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABPExample.Application.Interface;
using Coravel.Invocable;
using Quartz;
using Volo.Abp.DependencyInjection;

namespace Web.Common
{
    public class AppointmentJob: IAppointmentJob,IInvocable,ISingletonDependency
    {
        private readonly IAppointmentApplication _appointmentApplication;

        public AppointmentJob(IAppointmentApplication appointmentApplication)
        {
            _appointmentApplication = appointmentApplication;
        }
     
        public async Task Invoke()
        {
            await _appointmentApplication.UpdateAppointmentStatusAsync();
        }
    }
}
