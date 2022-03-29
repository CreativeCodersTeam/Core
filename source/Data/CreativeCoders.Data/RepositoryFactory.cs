using CreativeCoders.Core;
using JetBrains.Annotations;

namespace CreativeCoders.Data;

[PublicAPI]
public class RepositoryFactory : IRepositoryFactory
{
    private readonly IClassFactory _classFactory;

    public RepositoryFactory(IClassFactory classFactory)
    {
        _classFactory = classFactory;
    }

    public IRepository<TEntity> Create<TEntity>()
        where TEntity : class
    {
        return _classFactory.Create<IRepository<TEntity>>();
    }

    public IRepository<TKey, TEntity> Create<TKey, TEntity>()
        where TEntity : class, IEntityKey<TKey>
    {
        return _classFactory.Create<IRepository<TKey, TEntity>>();
    }
}