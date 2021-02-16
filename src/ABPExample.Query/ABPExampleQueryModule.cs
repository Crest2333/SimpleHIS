
using ABPExample.Domain;
using System;
using ABPExample.EntityFramework;
using ABPExample.Query.Common;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace ABPExample.Query
{
    [DependsOn(typeof(ABPExampleDomainModule)
      ,typeof(ABPExampleEntityFrameworkModule),typeof(AbpAutoMapperModule))]
    public class ABPExampleQueryModule:AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<ABPExampleQueryModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<ABPExampleQueryModule>();
            });
        }
    }
}
