using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;

namespace CreativeCoders.Data.EntityFramework;

public class EfRepository<TKey, TEntity> : IRepository<TKey, TEntity> where TEntity : class, IEntityKey<TKey>
{
    private readonly DbSet<TEntity> _dbSet;

    public EfRepository(DbSet<TEntity> dbSet)
    {
        _dbSet = dbSet;
    }

    public IIncludableDataQueryable<TEntity> QueryAll()
    {
        return new EfIncludableDataQueryable<TEntity>(_dbSet);
    }

    public IIncludableDataQueryable<TEntity> Query(Expression<Func<TEntity, bool>> expression)
    {
        return new EfIncludableDataQueryable<TEntity>(_dbSet.Where(expression));
    }

    public TEntity Get(Expression<Func<TEntity, bool>> expression)
    {
        return Query(expression).FirstOrDefault();
    }

    public TEntity Get(TKey id)
    {
        return _dbSet.Find(id);
    }

    public void Add(TEntity entity)
    {
        _dbSet.Add(entity);
    }

    public void Add(IEnumerable<TEntity> entities)
    {
        _dbSet.AddRange(entities);
    }

    public void Update(TEntity entity)
    {
        _dbSet.AddOrUpdate(entity);
    }

    public void Update(IEnumerable<TEntity> entities)
    {
        _dbSet.AddOrUpdate(entities.ToArray());
    }

    public void Delete(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    public void Delete(IEnumerable<TEntity> entities)
    {
        _dbSet.RemoveRange(entities);
    }
}