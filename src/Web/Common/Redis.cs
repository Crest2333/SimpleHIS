using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using HIS.Application.Common;
using Microsoft.Extensions.Configuration;
using Volo.Abp.DependencyInjection;

namespace Web.Common
{
    public class Redis: IRedis,ISingletonDependency
    {
        private readonly IConfiguration _configuration;

        public Redis(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public  RedisHelper GetRedisHelper()
        {
             var section = _configuration.GetSection("Redis:Default");
             //连接字符串
             string _connectionString = section.GetSection("Connection").Value;
             //实例名称
             string _instanceName = section.GetSection("InstanceName").Value;
             //默认数据库 
             int _defaultDB = int.Parse(section.GetSection("DefaultDB").Value ?? "0");
             return new RedisHelper(_connectionString, _instanceName, _defaultDB);
        }
    }
}
