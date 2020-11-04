using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.EntityFrameworkCore;

namespace ABPExample.EntityFramework.EntityFrameworkCore
{
    public  interface IBaseDbContext:IDisposable, ISingletonDependency
    {
        Task AddAsync<T>(T entity) where T : class;

        Task AddRangeAsync<T>(IList<T> entity) where T : class;

        void Update<T>(T entity) where T : class;

        void UpdateRange<T>(IList<T> entity) where T : class;

        Task SaveChangeAsync();

        void Modify<T, TPropty>(T entity, Expression<Func<T, TPropty>> expression) where T : class;

        Task UpdateSaveChangeAsync<T>(T entity) where T : class;

        Task UpdateRangeSaveChangeAsync<T>(IList<T> entity) where T : class;

        //void ModifyRange<T, TPorpty>(this IList<T> entity, Expression<Func<T, TPorpty> > func) where T: class;
    }
}
