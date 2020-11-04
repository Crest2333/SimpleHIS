using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ABPExample.EntityFramework.EntityFrameworkCore
{
    public class BaseDbContext : IBaseDbContext
    {
        private readonly IAppDbContext _context;

        public BaseDbContext(IAppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync<T>(T entity) where T : class
        {
            await _context.AddAsync(entity);
        }

        public async Task AddRangeAsync<T>(IList<T> entity) where T : class
        {
            await _context.AddRangeAsync(entity);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Modify<T, TPropty>(T entity, Expression<Func<T,TPropty>> expression) where T : class
        {
            _context.Attach(entity);
            _context.Entry(entity).Property(expression).IsModified = true;
        }

        public async Task SaveChangeAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }

        public async Task UpdateSaveChangeAsync<T>(T entity) where T : class
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }

        public void UpdateRange<T>(IList<T> entity) where T : class
        {
            _context.UpdateRange(entity);
        }

        public async Task UpdateRangeSaveChangeAsync<T>(IList<T> entity) where T : class
        {
            _context.UpdateRange(entity);
            await _context.SaveChangesAsync();
        }
    }
}
