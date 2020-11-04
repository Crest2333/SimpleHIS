using System;
using System.Collections.Generic;
using System.Text;
using Abp.Application.Services;
using ABPExample.Query.Interface;
using Volo.Abp.DependencyInjection;

namespace ABPExample.Query.Query
{
    public class PAQuery: ApplicationService, IPAQuery,ITransientDependency
    {

    }
}
