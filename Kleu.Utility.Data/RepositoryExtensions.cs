using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Kleu.Utility.Data
{
    public static class RepositoryExtensions
    {
        public static Task<IEnumerable<TEntity>> GetAsync<TEntity>(
            this IReadRepository<TEntity> repository,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> queryShaper)
            where TEntity : class, IEntity
        {
            Contract.Requires(repository != null);
            Contract.Requires(queryShaper != null);
            Contract.Ensures(Contract.Result<Task<IEnumerable<TEntity>>>() != null);

            return repository.GetAsync(queryShaper, CancellationToken.None);
        }

        public static Task<TResult> GetAsync<TEntity, TResult>(
            this IReadRepository<TEntity> repository,
            Func<IQueryable<TEntity>, TResult> queryShaper)
            where TEntity : class, IEntity
        {
            Contract.Requires(repository != null);
            Contract.Requires(queryShaper != null);
            Contract.Ensures(Contract.Result<Task<TResult>>() != null);

            return repository.GetAsync(queryShaper, CancellationToken.None);
        }

        public static Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(
            this IReadRepository<TEntity> repository)
            where TEntity : class, IEntity
        {
            Contract.Requires(repository != null);
            Contract.Ensures(Contract.Result<Task<IEnumerable<TEntity>>>() != null);

            return repository.GetAsync(query => query, CancellationToken.None);
        }

        public static Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(
            this IReadRepository<TEntity> repository,
            CancellationToken cancellationToken)
            where TEntity : class, IEntity
        {
            Contract.Requires(repository != null);
            Contract.Ensures(Contract.Result<Task<IEnumerable<TEntity>>>() != null);

            return repository.GetAsync(query => query, cancellationToken);
        }

        public static Task<IEnumerable<TEntity>> FindByAsync<TEntity>(
            this IReadRepository<TEntity> repository,
            Expression<Func<TEntity, bool>> predicate)
            where TEntity : class, IEntity
        {
            Contract.Requires(repository != null);
            Contract.Requires(predicate != null);
            Contract.Ensures(Contract.Result<Task<IEnumerable<TEntity>>>() != null);

            return repository.GetAsync(query => query.Where(predicate), CancellationToken.None);
        }

        public static Task<IEnumerable<TEntity>> FindByAsync<TEntity>(
            this IReadRepository<TEntity> repository,
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken)
            where TEntity : class, IEntity
        {
            Contract.Requires(repository != null);
            Contract.Requires(predicate != null);
            Contract.Ensures(Contract.Result<Task<IEnumerable<TEntity>>>() != null);

            return repository.GetAsync(query => query.Where(predicate), cancellationToken);
        }

        public static Task<TEntity> GetSingleAsync<TEntity>(
            this IReadRepository<TEntity> repository,
            Expression<Func<TEntity, bool>> predicate)
            where TEntity : class, IEntity
        {
            Contract.Requires(repository != null);
            Contract.Requires(predicate != null);
            Contract.Ensures(Contract.Result<Task<TEntity>>() != null);

            return repository.GetSingleAsync(predicate, CancellationToken.None);
        }

        public static async Task<TEntity> GetSingleAsync<TEntity>(
            this IReadRepository<TEntity> repository,
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken)
            where TEntity : class, IEntity
        {
            Contract.Requires(repository != null);
            Contract.Requires(predicate != null);
            Contract.Ensures(Contract.Result<Task<TEntity>>() != null);

            var items = await repository.GetAsync(query => query.Where(predicate), cancellationToken);
            return items.SingleOrDefault();
        }
    }
}