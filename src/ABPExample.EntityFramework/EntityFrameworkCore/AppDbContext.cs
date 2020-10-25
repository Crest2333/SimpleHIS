using ABPExample.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
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
