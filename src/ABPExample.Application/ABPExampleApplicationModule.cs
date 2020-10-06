using Volo.Abp.Modularity;
using ABPExample.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace ABPExample.Application
{
   [DependsOn(typeof(ABPExampleDomainModule))]
    public class ABPExampleApplicationModule:AbpModule
    {
      
    }
}
