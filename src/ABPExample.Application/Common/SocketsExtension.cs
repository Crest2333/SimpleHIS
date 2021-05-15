using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Autofac;
using HIS.Application.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace HIS.Application.Common
{
    public static class SocketsExtension
    {

        public static IServiceCollection AddWebSocketManager(this IServiceCollection services)
        {
            //var exportedTypes = AppDomain.CurrentDomain.GetAssemblies()
            //    .SelectMany(a => a.GetTypes().Where(t => t.GetBaseClasses().Contains(typeof(SocketsHandler))))
            //    .ToArray();
            //if (exportedTypes.Length==0) return services;

            //foreach (var type in exportedTypes)

            //    if (type.GetTypeInfo().BaseType == typeof(SocketsHandler))
            //    {
            //        ContainerBuilder builder = new ContainerBuilder();
            //        builder.RegisterType(type).SingleInstance();
            //        IContainer container = builder.Build();
            //    }
                   

            return services;
        }
 
        public static IApplicationBuilder MapSockets(this IApplicationBuilder app, PathString path,
            SocketsHandler socket)
        {
            return app.Map(path, x => x.UseMiddleware<SocketsMiddleware>(socket));
        }

    }
}
