
using ABPExample.Domain;
using System;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace ABPExample.Query
{
    [DependsOn(typeof(ABPExampleDomainModule)
        )]
    public class ABPExampleQueryModule:AbpModule
    {
    }
}
