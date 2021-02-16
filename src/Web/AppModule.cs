using ABPExample.Application;
using ABPExample.Domain;
using ABPExample.EntityFramework;
using ABPExample.Query;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABPExample.Query.Common;
using AutoMapper;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Web
{
    [DependsOn(typeof(AbpAspNetCoreMvcModule),
      typeof(AbpAutofacModule),
      typeof(ABPExampleEntityFrameworkModule),
      typeof(ABPExampleDomainModule),
      typeof(ABPExampleApplicationModule),
      typeof(ABPExampleQueryModule))]
    public class AppModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme) // Sets the default scheme to cookies
             .AddCookie(options =>
             {
                 options.AccessDeniedPath = "/Account/Login";
                 options.LoginPath = "/Account/Login";
                 options.LogoutPath = "/Account/Logout";
             });
        }
        public override void OnApplicationInitialization(
            ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            var env = context.GetEnvironment();


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseConfiguredEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Account}/{action=Login}/{id?}");
            });
        }
    }
}
