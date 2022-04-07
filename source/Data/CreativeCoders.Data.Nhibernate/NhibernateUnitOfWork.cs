using System;
using System.Data;
using CreativeCoders.Core;
using JetBrains.Annotations;
using NHibernate;

namespace CreativeCoders.Data.Nhibernate;

[PublicAPI]
public class NhibernateUnitOfWork : UnitOfWorkBase
{
    private readonly ISession _session;

    private readonly ITransaction _transaction;

    public NhibernateUnitOfWork(ISessionFactory sessionFactory)
    {
        Ensure.IsNotNull(sessionFactory, "sessionFactory");

        _session = sessionFactory.OpenSession();
        _transaction = _session.BeginTransaction(IsolationLevel.ReadCommitted);
    }

    protected override IRepository<TKey, TEntity> CreateRepository<TKey, TEntity>()
    {
        return new NhibernateRepository<TKey, TEntity>(_session);
    }

    protected override void Dispose(bool disposing)
    {
        if (!disposing)
        {
            return;
        }

        if (_session.IsOpen)
        {
            _session.Close();
        }

        GC.SuppressFinalize(this);
    }

    public override void Commit()
    {
        if (!_transaction.IsActive)
        {
            throw new InvalidOperationException("Transaction is not active.");
        }

        _transaction.Commit();
    }

    public override void Rollback()
    {
        if (_transaction.IsActive)
        {
            _transaction.Rollback();
        }
    }
}
