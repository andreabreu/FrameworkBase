using Framework.Core.Repository.Interfaces;
using MongoDB.Driver;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Core.Repository
{
    public abstract class BaseMongoRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly IMongoContext _context;
        protected IMongoCollection<TEntity> DbSet;

        protected BaseMongoRepository(IMongoContext context)
        {
            _context = context;

            DbSet = _context.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public virtual async void Add(TEntity obj)
        {
            await DbSet.InsertOneAsync(obj);
        }

        public virtual async Task<TEntity> GetById(Guid id)
        {
            var data = await DbSet.FindAsync(Builders<TEntity>.Filter.Eq("_id", id));
            return data.SingleOrDefault();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            var all = await DbSet.FindAsync(Builders<TEntity>.Filter.Empty);
            return all.ToList();
        }

        public virtual async void Update(TEntity obj)
        {
            await DbSet.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("_id", obj.GetId()), obj);
        }

        public virtual async Task Remove(Guid id)
        {
            await DbSet.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", id));
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
