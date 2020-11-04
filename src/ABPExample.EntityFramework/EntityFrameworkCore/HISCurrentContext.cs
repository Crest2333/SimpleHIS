using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.EntityFrameworkCore;

namespace ABPExample.EntityFramework.EntityFrameworkCore
{
    public class HISCurrentContext:IHISCurrentContext,IDisposable
    {
        private readonly AppDbContext context;

        public HISCurrentContext(DbContextPool<AppDbContext> pool)
        {
            context = pool.Rent();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IHISCurrentContext GetConext()
        {
            throw new NotImplementedException();
        }
    }
}
