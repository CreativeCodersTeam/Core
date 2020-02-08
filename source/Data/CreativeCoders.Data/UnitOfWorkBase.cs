using JetBrains.Annotations;

namespace CreativeCoders.Data
{
    [PublicAPI]
    public abstract class UnitOfWorkBase : IUnitOfWork
    {
        private bool _isDisposed;

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;
            Dispose(true);
        }

        protected abstract void Dispose(bool disposing);

        public abstract void Commit();

        public abstract void Rollback();

        protected abstract IRepository<TKey, TEntity> CreateRepository<TKey, TEntity>()
            where TEntity : class, IEntityKey<TKey>;
    }
}
