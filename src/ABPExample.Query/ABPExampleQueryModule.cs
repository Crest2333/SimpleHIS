
using ABPExample.Domain;
using System;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace ABPExample.Query
{
    [DependsOn(typeof(ABPExampleDomainModule)
        ,typeof(AbpAutoMapperModule))]
    public class ABPExampleQueryModule:AbpModule
    {
    }
}
