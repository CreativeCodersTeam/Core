using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CreativeCoders.Core;
using NHibernate;

namespace CreativeCoders.Data.Nhibernate;

public class NhibernateRepository<TKey, TEntity> : IRepository<TKey, TEntity>
    where TEntity : class, IEntityKey<TKey>
{
    private readonly ISession _session;

    public NhibernateRepository(ISession session)
    {
        Ensure.IsNotNull(session, "session");

        _session = session;
    }

    public IIncludableDataQueryable<TEntity> QueryAll()
    {
        return new NhibernateIncludableDataQueryable<TEntity>(_session.Query<TEntity>());
    }

    public IIncludableDataQueryable<TEntity> Query(Expression<Func<TEntity, bool>> expression)
    {
        return new NhibernateIncludableDataQueryable<TEntity>(QueryAll().Where(expression));
    }

    public TEntity Get(Expression<Func<TEntity, bool>> expression)
    {
        return Query(expression).FirstOrDefault();
    }

    public TEntity Get(TKey id)
    {
        return _session.Get<TEntity>(id);
    }

    public void Add(TEntity entity)
    {
        _session.Save(entity);
    }

    public void Add(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            Add(entity);
        }
    }

    public void Update(TEntity entity)
    {
        _session.Update(entity);
    }

    public void Update(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            Update(entity);
        }
    }

    public void Delete(TEntity entity)
    {
        _session.Delete(entity);
    }

    public void Delete(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            Delete(entity);
        }
    }
}
