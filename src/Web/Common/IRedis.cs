using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIS.Application.Common;

namespace Web.Common
{
    public interface IRedis
    {
        RedisHelper GetRedisHelper();
    }
}
