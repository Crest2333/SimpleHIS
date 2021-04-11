using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace HIS.EntityFramework.Common
{
    public static class BaseCurrentContext
    {
        public static void Modified<TEntity>(this DbContext content, TEntity entity, Type prop)
        {
            content.Entry(entity).Property(prop.ToString()).IsModified = true;
        }
    }
}
