using System.Threading.Tasks;

namespace Kleu.Utility.Data
{
    public interface ICreateRepository<in TEntity, TKey> : IRepository
        where TEntity : class, IEntity
    {
        Task<TKey> Create(TEntity entity);
    }
}
