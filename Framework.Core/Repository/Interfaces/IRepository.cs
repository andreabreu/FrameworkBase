using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Framework.Core.Repository.Interfaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        void Add(TEntity obj);
        Task<TEntity> GetById(Guid id);
        Task<IEnumerable<TEntity>> GetAll();
        void Update(TEntity obj);
        Task Remove(Guid id);
    }
}
