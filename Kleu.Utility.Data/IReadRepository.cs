using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kleu.Utility.Data
{
    public interface IReadRepository<TEntity> : IRepository
        where TEntity : class, IEntity
    {
        Task<IEnumerable<TEntity>> GetAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryBuilder, CancellationToken cancellationToken);

        Task<TResult> GetAsync<TResult>(Func<IQueryable<TEntity>, TResult> queryBuilder, CancellationToken cancellationToken);
    }
}
