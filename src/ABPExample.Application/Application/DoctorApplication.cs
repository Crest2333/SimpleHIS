using ABPExample.Application.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.DependencyInjection;

namespace ABPExample.Application.Application
{
    public class DoctorApplication:IDoctorApplication, ITransientDependency
    {

    }
}
