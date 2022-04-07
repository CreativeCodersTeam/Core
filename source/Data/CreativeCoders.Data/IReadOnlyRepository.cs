using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace CreativeCoders.Data;

[PublicAPI]
public interface IReadOnlyRepository<TEntity>
    where TEntity : class
{
    IIncludableDataQueryable<TEntity> QueryAll();

    IIncludableDataQueryable<TEntity> Query(Expression<Func<TEntity, bool>> expression);

    TEntity Get(Expression<Func<TEntity, bool>> expression);
}

[PublicAPI]
public interface IReadOnlyRepository<in TKey, TEntity> : IReadOnlyRepository<TEntity>
    where TEntity : class, IEntityKey<TKey>
{
    TEntity Get(TKey id);
}
