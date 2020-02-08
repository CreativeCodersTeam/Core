using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CreativeCoders.Data.Ndatabase
{
    public class NdatabaseIncludableDataQueryable<TEntity> : IIncludableDataQueryable<TEntity>
        where TEntity : class
    {
        private readonly IQueryable<TEntity> _entities;

        public NdatabaseIncludableDataQueryable(IQueryable<TEntity> entities)
        {
            _entities = entities;
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return _entities.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IIncludableDataQueryable<TEntity, TProperty> Include<TProperty>(Expression<Func<TEntity, TProperty>> includeProperty)
            where TProperty : class
        {
            return new NdatabaseIncludableDataQueryable<TEntity, TProperty>(_entities);
        }

        public IIncludableDataQueryable<TEntity, IEnumerable<TProperty>, TProperty> IncludeCollection<TProperty>(Expression<Func<TEntity, IEnumerable<TProperty>>> includeProperty)
            where TProperty : class
        {
            return new NdatabaseIncludableDataQueryable<TEntity, IEnumerable<TProperty>, TProperty>(_entities);
        }

        public Type ElementType => _entities.ElementType;

        public Expression Expression => _entities.Expression;

        public IQueryProvider Provider => _entities.Provider;
    }



    public class NdatabaseIncludableDataQueryable<TEntity, TProperty> : NdatabaseIncludableDataQueryable<TEntity>, IIncludableDataQueryable<TEntity, TProperty>
        where TEntity : class
        where TProperty : class
    {
        private readonly IQueryable<TEntity> _subEntities;

        public NdatabaseIncludableDataQueryable(IQueryable<TEntity> subEntities) : base(subEntities)
        {
            _subEntities = subEntities;
        }

        public IIncludableDataQueryable<TEntity, TSubProperty> ThenInclude<TSubProperty>(Expression<Func<TProperty, TSubProperty>> includeSubProperty) where TSubProperty : class
        {
            return new NdatabaseIncludableDataQueryable<TEntity, TSubProperty>(_subEntities);
        }

        public IIncludableDataQueryable<TEntity, IEnumerable<TSubProperty>, TSubProperty> ThenIncludeCollection<TSubProperty>(Expression<Func<TProperty, IEnumerable<TSubProperty>>> includeSubPropertyCollection) where TSubProperty : class
        {
            return new NdatabaseIncludableDataQueryable<TEntity, IEnumerable<TSubProperty>, TSubProperty>(_subEntities);
        }
    }

    public class NdatabaseIncludableDataQueryable<TEntity, TCollection, TProperty> : IIncludableDataQueryable<TEntity, TCollection, TProperty>
        where TCollection : IEnumerable<TProperty> where TEntity : class where TProperty : class
    {
        private readonly IQueryable<TEntity> _subEntities;

        public NdatabaseIncludableDataQueryable(IQueryable<TEntity> subEntities)
        {
            _subEntities = subEntities;
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return _subEntities.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Type ElementType => _subEntities.ElementType;

        public Expression Expression => _subEntities.Expression;

        public IQueryProvider Provider => _subEntities.Provider;

        public IIncludableDataQueryable<TEntity, TProperty1> Include<TProperty1>(Expression<Func<TEntity, TProperty1>> includeProperty) where TProperty1 : class
        {
            return new NdatabaseIncludableDataQueryable<TEntity, TProperty1>(_subEntities);
        }

        public IIncludableDataQueryable<TEntity, IEnumerable<TProperty1>, TProperty1> IncludeCollection<TProperty1>(Expression<Func<TEntity, IEnumerable<TProperty1>>> includeProperty) where TProperty1 : class
        {
            return new NdatabaseIncludableDataQueryable<TEntity, IEnumerable<TProperty1>, TProperty1>(_subEntities);
        }

        public IIncludableDataQueryable<TEntity, TSubProperty> ThenInclude<TSubProperty>(Expression<Func<TProperty, TSubProperty>> includeSubProperty) where TSubProperty : class
        {
            return new NdatabaseIncludableDataQueryable<TEntity, TSubProperty>(_subEntities);
        }

        public IIncludableDataQueryable<TEntity, IEnumerable<TSubProperty>, TSubProperty> ThenIncludeCollection<TSubProperty>(Expression<Func<TProperty, IEnumerable<TSubProperty>>> includeSubPropertyCollection) where TSubProperty : class
        {
            return new NdatabaseIncludableDataQueryable<TEntity, IEnumerable<TSubProperty>, TSubProperty>(_subEntities);
        }
    }
}