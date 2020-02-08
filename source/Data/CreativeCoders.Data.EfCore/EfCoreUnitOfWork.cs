using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace CreativeCoders.Data.EfCore
{
    [PublicAPI]
    public class EfCoreUnitOfWork : UnitOfWorkBase
    {
        private readonly DbContext _dbContext;

        public EfCoreUnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected override void Dispose(bool disposing)
        {
            _dbContext.Dispose();
        }

        public override void Commit()
        {
            _dbContext.SaveChanges();
            if (_dbContext.Database.CurrentTransaction != null)
            {
                _dbContext.Database.CommitTransaction();
            }
        }

        public override void Rollback()
        {
            _dbContext.Database.RollbackTransaction();
        }

        protected override IRepository<TKey, TEntity> CreateRepository<TKey, TEntity>()
        {
            return new EfCoreRepository<TKey, TEntity>(_dbContext);
        }
    }
}