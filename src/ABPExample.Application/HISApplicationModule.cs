using Volo.Abp.Modularity;
using ABPExample.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp;

namespace ABPExample.Application
{
   [DependsOn(typeof(HISDomainModule))]
    public class HISApplicationModule:AbpModule
    {
        
    }
}
