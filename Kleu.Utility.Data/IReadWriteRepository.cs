namespace Kleu.Utility.Data
{
    public interface IReadWriteRepository<TEntity, TKey> :
        IReadRepository<TEntity>,
        IUpdateRepository<TEntity>,
        ICreateRepository<TEntity, TKey>,
        IDeleteRepository<TKey>
        where TEntity : class, IEntity
    {

    }
}
