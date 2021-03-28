using System;
using System.Collections.Generic;
using System.Text;
using ABPExample.EntityFramework.EntityFrameworkCore;
using ABPExample.Query.Interface;
using Volo.Abp.DependencyInjection;

namespace ABPExample.Query.Query
{
    public class CommonQuery:ICommonQuery,ITransientDependency
    {
        private readonly IAppDbContext _context;

        public CommonQuery(IAppDbContext context)
        {
            _context = context;
        }


    }
}
