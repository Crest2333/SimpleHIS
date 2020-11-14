using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.DependencyInjection;

namespace ABPExample.EntityFramework.EntityFrameworkCore
{
    public class HISContextProvider : IHISContextProvider
    {
        private readonly DbContextPool<AppDbContext> _dbContextPool;
        public HISContextProvider()
        {
            _dbContextPool = GetContextPool();
        }
        public IHISCurrentContext GetConext()
        {
            return new HISCurrentContext(_dbContextPool);
        }

        private static DbContextPool<AppDbContext> GetContextPool()
        {
            const string connectionString = "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=HIS;Data Source=.";
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.UseSqlServer(connectionString, options => options.EnableRetryOnFailure());
            builder.EnableSensitiveDataLogging();

            ((IDbContextOptionsBuilderInfrastructure)builder)
                .AddOrUpdateExtension(builder.Options.FindExtension<CoreOptionsExtension>().WithMaxPoolSize(10));
            return new DbContextPool<AppDbContext>(builder.Options);
        }
    }
}
