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
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using ABPExample.EntityFramework.Common;
using ABPExample.Query.Common;
using AutoMapper;
using Coravel;
using HIS.Application.Common;
using HIS.Application.Hubs;
using HIS.Application.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Quartz;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Web.Common;

namespace Web
{
    [DependsOn(typeof(AbpAspNetCoreMvcModule),
      typeof(AbpAutofacModule),
      typeof(HISEntityFrameworkModule),
      typeof(HISDomainModule),
      typeof(HISApplicationModule),
      typeof(HISQueryModule))]
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
            DiagnosticListener.AllListeners.Subscribe(new CommandListener());
            context.Services.AddWebSocketManager();
            context.Services.AddSignalR();
            
            context.Services.AddMvc(options =>
            {

            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc; // 设置时区为 UTC)
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });
            context.Services.AddScheduler();
        }
        public override void OnApplicationInitialization(
            ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            var env = context.GetEnvironment();

            var provider = app.ApplicationServices;
            provider.UseScheduler(scheduler =>
            {
                scheduler.Schedule<AppointmentJob>()
                    .EveryTenMinutes();
            });
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
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(),"Images" )),
                RequestPath = "/Images"
            });
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseWebSockets();
            //app.UseMiddleware<SocketsMiddleware>(context.ServiceProvider.GetService<WebSocketMessageHandler>());
            app.MapSockets("/ws", context.ServiceProvider.GetService<WebSocketMessageHandler>());
            
            app.UseConfiguredEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Account}/{action=Login}/{id?}");
                endpoints.MapHub<ChatHub>("/chatHub");
            });
        }

        
    }
}
