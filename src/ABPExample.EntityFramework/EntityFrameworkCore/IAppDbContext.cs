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
    public interface IAppDbContext : IEfCoreDbContext, ISingletonDependency
    {
        DbSet<Users> Users { get; set; }

        DbSet<Role> Role { get; set; }

        DbSet<RoleMapper> RoleMapper { get; set; }

        DbSet<DepartmentMapper> DepartmentMapper { get; set; }

        DbSet<Department> Department { get; set; }

        DbSet<Patients> Patients { get; set; }

        DbSet<PastHistories> PastHistories { get; set; }

        DbSet<Drug> Drug { get; set; }

        DbSet<DrugCompatibility> DrugCompatibility { get; set; }

        DbSet<Appointment> Appointment { get; set; }

        DbSet<TestExport> TestExport { get; set; }

    }
}
