using System.Data;
using System.Data.Entity;
using CreativeCoders.Core;
using JetBrains.Annotations;

namespace CreativeCoders.Data.EntityFramework;

[PublicAPI]
public class EfUnitOfWork : UnitOfWorkBase
{
    private readonly DbContext _dbContext;

    private readonly DbContextTransaction _transaction;

    public EfUnitOfWork(DbContext dbContext)
    {
        Ensure.IsNotNull(dbContext, nameof(dbContext));

        _dbContext = dbContext;
        _transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadCommitted);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _dbContext.Dispose();
        }
    }

    public override void Commit()
    {
        _dbContext.SaveChanges();
        _transaction.Commit();
    }

    public override void Rollback()
    {
        _transaction.Rollback();
    }

    protected override IRepository<TKey, TEntity> CreateRepository<TKey, TEntity>()
    {
        return new EfRepository<TKey, TEntity>(_dbContext.Set<TEntity>());
    }
}