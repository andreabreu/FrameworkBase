using Framework.Core.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Framework.Core.Repository
{
    public class BaseSqlRepository<TEntity> : IRepository<TEntity> where TEntity : class, IUnitOfWork
    {
        protected readonly DbContext _context;

        public BaseSqlRepository(DbContext context)
        {
            _context = context;
        }

        public virtual void Add(TEntity obj)
        {
            _context.Set<TEntity>().Add(obj);
            _context.SaveChanges();
        }

        public virtual async Task<TEntity> GetById(Guid id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public void Update(TEntity obj)
        {
            _context.Entry(obj).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public async Task Remove(Guid id)
        {
            _context.Set<TEntity>().Remove(await GetById(id));
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
