using System.Threading.Tasks;

namespace Kleu.Utility.Data
{
    public interface IUpdateRepository<in TEntity> : IRepository
        where TEntity : class, IEntity
    {
        Task<bool> Update(TEntity entity);
    }
}
