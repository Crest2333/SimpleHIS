using ABPExample.Domain;
using ABPExample.EntityFramework.EntityFrameworkCore;

using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Modularity;

namespace ABPExample.EntityFramework
{
    [DependsOn(
        typeof(HISDomainModule),
        typeof(AbpEntityFrameworkCoreSqlServerModule)
        )]
    public class HISEntityFrameworkModule:AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            //添加上下文类的依赖
            context.Services.AddAbpDbContext<AppDbContext>(options=> {
                options.AddDefaultRepositories();
            });
            Configure<AbpDbContextOptions>(options =>
            {
                //改变使用的数据库
                options.UseSqlServer();
            });
        }

    }
}
