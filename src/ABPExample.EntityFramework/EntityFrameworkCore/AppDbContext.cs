﻿using ABPExample.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using HIS.Domain.Models;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace ABPExample.EntityFramework.EntityFrameworkCore
{
    /*这是运行时使用的实际DbContext。

    *它只包括你的实体。

    *它不包括所用模块的实体，因为每个模块已经

    *它自己的DbContext类。如果要与使用的模块共享一些数据库表，

    *只需为AppUser创建一个类似done的结构。

    *不要将此DbContext用于数据库迁移，因为它不包含

    *使用过的模块（如上所述）。有关迁移，请参阅HISMigrationsDbContext。

    */
    [ConnectionStringName("DefaultConn")]
    public class AppDbContext : AbpDbContext<AppDbContext>,IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        public DbSet<Users> Users { get; set; }

        public DbSet<Role> Role { get; set ; }

        public DbSet<RoleMapper> RoleMapper { get ; set ; }

        public DbSet<DepartmentMapper> DepartmentMapper { get; set; }

        public DbSet<Department> Department { get; set; }

        public DbSet<Patients> Patients { get; set; }

        public DbSet<PastHistories> PastHistories { get; set; }

        public DbSet<Drug> Drug { get; set; }

        public DbSet<DrugCompatibility> DrugCompatibility { get; set; }

        public DbSet<Appointment> Appointment { get; set; }

        public DbSet<Scheduling> Scheduling { get; set; }

        public DbSet<TestExport> TestExport { get ; set; }

        public DbSet<MedicalAdvice> MedicalAdvice { get; set; }

        public DbSet<PatientUser> PatientUser { get; set; }

        public DbSet<PatientsMapping> PatientsMapping { get; set; }

        public DbSet<Doctor> Doctor { get; set; }

        public DbSet<ChatLog> ChatLog { get; set; }

        public void Modified<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> func)where  TEntity:class
        {
            base.Entry(entity).Property(func).IsModified = true;
        }

        public void ModifiedRange<TEntity, TProperty>(IEnumerable<TEntity> entityList, Expression<Func<TEntity, TProperty>> func) where  TEntity:class
        {
            foreach (var entity in entityList)
                base.Entry(entity).Property(func).IsModified = true;
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Users>(c =>
            {
                c.ConfigureByConvention();
            });
        }


    }
}
