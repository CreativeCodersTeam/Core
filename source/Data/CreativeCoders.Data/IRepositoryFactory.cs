using JetBrains.Annotations;

namespace CreativeCoders.Data;

[PublicAPI]
public interface IRepositoryFactory
{
    IRepository<TEntity> Create<TEntity>()
        where TEntity : class;

    IRepository<TKey, TEntity> Create<TKey, TEntity>()
        where TEntity : class, IEntityKey<TKey>;
}
