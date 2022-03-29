using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CreativeCoders.Core;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace CreativeCoders.Data.EfCore;

[PublicAPI]
public class EfCoreRepository<TEntity> : IRepository<TEntity>
    where TEntity : class
{
    public EfCoreRepository(DbContext dbContext)
    {
        Ensure.IsNotNull(dbContext, nameof(dbContext));

        DbSet = dbContext.Set<TEntity>();
    }

    public IIncludableDataQueryable<TEntity> QueryAll()
    {
        return new EfCoreIncludableDataQueryable<TEntity>(DbSet);
    }

    public IIncludableDataQueryable<TEntity> Query(Expression<Func<TEntity, bool>> expression)
    {
        return new EfCoreIncludableDataQueryable<TEntity>(DbSet.Where(expression));
    }

    public TEntity Get(Expression<Func<TEntity, bool>> expression)
    {
        return Query(expression).FirstOrDefault();
    }

    public void Add(TEntity entity)
    {
        DbSet.Add(entity);
    }

    public void Add(IEnumerable<TEntity> entities)
    {
        DbSet.AddRange(entities);
    }

    public void Update(TEntity entity)
    {
        DbSet.Update(entity);
    }

    public void Update(IEnumerable<TEntity> entities)
    {
        DbSet.UpdateRange(entities);
    }

    public void Delete(TEntity entity)
    {
        DbSet.Remove(entity);
    }

    public void Delete(IEnumerable<TEntity> entities)
    {
        DbSet.RemoveRange(entities);
    }

    public DbSet<TEntity> DbSet { get; }
}

public class EfCoreRepository<TKey, TEntity> : EfCoreRepository<TEntity>, IRepository<TKey, TEntity>
    where TEntity : class, IEntityKey<TKey>
{
    public EfCoreRepository(DbContext dbContext) : base(dbContext)
    {
    }

    public TEntity Get(TKey id)
    {
        return DbSet.Find(id);
    }
}