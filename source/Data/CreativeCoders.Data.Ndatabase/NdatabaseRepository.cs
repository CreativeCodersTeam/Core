using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CreativeCoders.Core;
using NDatabase;
using NDatabase.Api;

namespace CreativeCoders.Data.Ndatabase
{
    public class NdatabaseRepository<TKey, TEntity> : IRepository<TKey, TEntity> where TEntity : class, IEntityKey<TKey>
    {
        private readonly IOdb _odb;

        public NdatabaseRepository() : this(OdbFactory.OpenInMemory()) {}

        public NdatabaseRepository(IOdb odb)
        {
            Ensure.IsNotNull(odb, nameof(odb));

            _odb = odb;
        }

        public NdatabaseRepository(string dbFileName) : this(OdbFactory.Open(dbFileName)) {}

        public IIncludableDataQueryable<TEntity> QueryAll()
        {
            return new NdatabaseIncludableDataQueryable<TEntity>(_odb.AsQueryable<TEntity>());
        }

        public IIncludableDataQueryable<TEntity> Query(Expression<Func<TEntity, bool>> expression)
        {
            return new NdatabaseIncludableDataQueryable<TEntity>(QueryAll().Where(expression));
        }

        public TEntity Get(Expression<Func<TEntity, bool>> expression)
        {
            return Query(expression).FirstOrDefault();
        }

        public TEntity Get(TKey id)
        {
            return Query(x => x.Id.Equals(id)).FirstOrDefault();
        }

        public void Add(TEntity entity)
        {
            _odb.Store(entity);
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
            _odb.Store(entity);
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
            _odb.Delete(entity);
        }

        public void Delete(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }
    }
}