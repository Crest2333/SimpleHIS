using ABPExample.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using HIS.Domain.Models;
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

        DbSet<Scheduling> Scheduling { get; set; }

        DbSet<TestExport> TestExport { get; set; }

        DbSet<MedicalAdvice> MedicalAdvice { get; set; }

        DbSet<PatientUser> PatientUser { get; set; }

        DbSet<PatientsMapping> PatientsMapping { get; set; }

        DbSet<Doctor> Doctor { get; set; }

        DbSet<ChatLog> ChatLog { get; set; }

        void Modified<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> func) where TEntity : class;

        void ModifiedRange<TEntity, TProperty>(IEnumerable<TEntity> entity, Expression<Func<TEntity, TProperty>> func) where TEntity : class;


    }
}
