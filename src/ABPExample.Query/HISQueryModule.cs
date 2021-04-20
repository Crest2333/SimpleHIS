
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
    [DependsOn(typeof(HISDomainModule)
      ,typeof(HISEntityFrameworkModule),typeof(AbpAutoMapperModule))]
    public class HISQueryModule:AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<HISQueryModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<HISQueryModule>();
            });
        }
    }
}
