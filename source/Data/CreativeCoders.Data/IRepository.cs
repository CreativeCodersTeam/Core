namespace CreativeCoders.Data
{
    public interface IRepository<TEntity> : IReadOnlyRepository<TEntity>, IPersistRepository<TEntity>
        where TEntity : class
    {
    }

    public interface IRepository<in TKey, TEntity> : IRepository<TEntity>, IReadOnlyRepository<TKey, TEntity>
        where TEntity : class, IEntityKey<TKey>
    {
    }
}