using System.Threading.Tasks;

namespace Framework.Core.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
