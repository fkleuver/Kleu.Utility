using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Kleu.Utility.Data.Exceptions;
using Kleu.Utility.Logging;
using Newtonsoft.Json;
// ReSharper disable MemberCanBePrivate.Global

namespace Kleu.Utility.Data
{
    public abstract class BaseReadWriteRepository<TContext, TEntity, TOutput> :
        ICreateRepository<TEntity, TOutput>,
        IReadRepository<TEntity>,
        IDeleteRepository<TOutput>,
        IUpdateRepository<TEntity>
        where TEntity : class, IEntity
        where TOutput : class, IEntity
        where TContext : IDbContext
    {
        private readonly ILog _logger;
        protected readonly Func<TContext> ContextFactory;

        protected BaseReadWriteRepository(ILog logger, Func<TContext> contextFactory)
        {
            _logger = logger;
            ContextFactory = contextFactory;
        }

        public async Task<IEnumerable<TEntity>> GetAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryBuilder, CancellationToken cancellationToken)
        {
            _logger.Info($"Retrieving {typeof(TEntity).Name}");

            var ctx = ContextFactory();

            var query = queryBuilder(ctx.Set<TEntity>());
            try
            {
                return await query.ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.ErrorException($"Exception on trying to retrieve {typeof(TEntity).Name}: ", ex);
                throw new EntityRetrievalException(ex);

            }
        }


        public async Task<TResult> GetAsync<TResult>(Func<IQueryable<TEntity>, TResult> queryBuilder, CancellationToken cancellationToken)
        {
            _logger.Info($"Retrieving {typeof(TEntity).Name}");

            var ctx = ContextFactory();

            try
            {
                var factory = Task<TResult>.Factory;
                return await factory.StartNew(() => queryBuilder(ctx.Set<TEntity>()), cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.ErrorException($"Exception on trying to retrieve {typeof(TEntity).Name}: ", ex);
                throw new EntityRetrievalException(ex);
            }
        }

        public async Task<TOutput> Create(TEntity entity)
        {
            var ctx = ContextFactory();
            var entityTypeName = typeof(TEntity).Name;
            TEntity result;

            try
            {
                var output = ctx.Set<TEntity>();
                result = output.Add(entity);
            }
            catch (Exception ex)
            {
                _logger.ErrorException($"Exception on trying to add {entityTypeName} to DbSet: ", ex);
                throw new AddEntityToDbContextException(ex);
            }

            try
            {
                await ctx.SaveChangesAsync();

                try
                {
                    _logger.Info($"Successfully saved Entity (Id = {entity.Id})");
                }
                catch
                {
                    // ignored
                }
            }
            catch (DbUpdateException ex)
            {
                _logger.ErrorException($"DbUpdateException on creating {entityTypeName}: ", ex);
                throw new EntityCreationException(ex);
            }
            catch (DbEntityValidationException ex)
            {
                _logger.ErrorException($"Exception on creating entity {entityTypeName}: ", ex);
                var sb = new StringBuilder();
                sb.AppendLine("-- [Validation errors]:");

                foreach (var eve in ex.EntityValidationErrors)
                {
                    sb.AppendLine($"---- [Entity]: {JsonConvert.SerializeObject(eve.Entry.Entity)}");
                    foreach (var ve in eve.ValidationErrors)
                    {
                        sb.AppendLine($"------ [{ve.PropertyName}]: {ve.ErrorMessage}");
                    }
                }

                _logger.Error(sb.ToString);
                throw new EntityCreationValidationException(ex);
            }
            catch (Exception ex)
            {
                _logger.FatalException($"Fatal exception on creating {entityTypeName}: ", ex);
                throw new EntityCreationException(ex);
            }

            return result as TOutput;
        }

        public async Task<bool> Delete(TOutput entity)
        {
            var ctx = ContextFactory();
            ctx.Set<TOutput>().Attach(entity);
            ctx.Entry(entity).State = EntityState.Deleted;

            try
            {
                await ctx.SaveChangesAsync();
                _logger.Info($"Successfully deleted {typeof(TEntity).Name} (Id: {entity.Id})");
            }
            catch (DbEntityValidationException ex)
            {
                _logger.ErrorException($"DbEntityValidationException on deleting {typeof(TOutput).Name} (Id: {entity.Id}): ", ex);
                throw new EntityDeleteValidationException(ex);
            }
            catch (Exception ex)
            {
                _logger.ErrorException($"{ex.GetType()} on deleting {typeof(TOutput).Name} (Id: {entity.Id}): ", ex);
                throw new EntityDeleteException(ex);

            }

            return true;
        }

        public async Task<bool> Update(TEntity entity)
        {
            var ctx = ContextFactory();
            ctx.Entry(entity).State = EntityState.Modified;

            try
            {
                await ctx.SaveChangesAsync();
                _logger.Info($"Successfully updated {typeof(TEntity).Name} (Id: {entity.Id})");
            }
            catch (DbEntityValidationException ex)
            {
                _logger.ErrorException($"DbEntityValidationException on updating {typeof(TEntity).Name} (Id: {entity.Id}): ", ex);
                ctx.Entry(entity).State = EntityState.Unchanged;
                throw new EntityUpdateValidationException(ex);
            }
            catch (Exception ex)
            {
                _logger.ErrorException($"{ex.GetType()} on updating {typeof(TEntity).Name} (Id: {entity.Id}): ", ex);
                throw new EntityUpdateException(ex);

            }

            return true;
        }
    }
}
