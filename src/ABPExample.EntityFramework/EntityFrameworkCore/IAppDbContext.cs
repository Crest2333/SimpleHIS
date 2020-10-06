using ABPExample.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace ABPExample.EntityFramework.EntityFrameworkCore
{
    [ConnectionStringName("DefaultConn")]
    public interface IAppDbContext:IEfCoreDbContext,ISingletonDependency
    {
         DbSet<Users> Users { get; set; }
    }
}
