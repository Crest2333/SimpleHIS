using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Web.Common
{
    public class MyController:AbpController
    {
        public override JsonResult Json(object data)
        {
            return base.Json(data);
        }
    }
}
