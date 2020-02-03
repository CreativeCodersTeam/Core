using CreativeCoders.Core;
using JetBrains.Annotations;
using NDatabase;
using NDatabase.Api;

namespace CreativeCoders.Data.Ndatabase
{
    [PublicAPI]
    public class NdatabaseUnitOfWork : UnitOfWorkBase
    {
        private readonly IOdb _odb;

        public NdatabaseUnitOfWork() : this(OdbFactory.OpenInMemory()) {}

        public NdatabaseUnitOfWork(IOdb odb)
        {
            Ensure.IsNotNull(odb, nameof(odb));

            _odb = odb;
        }

        public NdatabaseUnitOfWork(string dbFileName) : this(OdbFactory.Open(dbFileName)) {}

        protected override IRepository<TKey, TEntity> CreateRepository<TKey, TEntity>()
        {
            return new NdatabaseRepository<TKey, TEntity>(_odb);
        }

        protected override void Dispose(bool disposing)
        {
            
        }

        public override void Commit()
        {
            
        }

        public override void Rollback()
        {
            
        }
    }
}
