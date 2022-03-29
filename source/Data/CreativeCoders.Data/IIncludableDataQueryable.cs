using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace CreativeCoders.Data;

[PublicAPI]
public interface IIncludableDataQueryable<TEntity> : IQueryable<TEntity>
    where TEntity : class
{
    IIncludableDataQueryable<TEntity, TProperty> Include<TProperty>(Expression<Func<TEntity, TProperty>> includeProperty)
        where TProperty : class;

    IIncludableDataQueryable<TEntity, IEnumerable<TProperty>, TProperty> IncludeCollection<TProperty>(Expression<Func<TEntity, IEnumerable<TProperty>>> includeProperty)
        where TProperty : class;
}

[PublicAPI]
public interface IIncludableDataQueryable<TEntity, TProperty> : IIncludableDataQueryable<TEntity>
    where TEntity : class
    where TProperty : class
{
    IIncludableDataQueryable<TEntity, TSubProperty> ThenInclude<TSubProperty>(Expression<Func<TProperty, TSubProperty>> includeSubProperty)
        where TSubProperty : class;

    IIncludableDataQueryable<TEntity, IEnumerable<TSubProperty>, TSubProperty> ThenIncludeCollection<TSubProperty>(
        Expression<Func<TProperty, IEnumerable<TSubProperty>>> includeSubPropertyCollection)
        where TSubProperty : class;
}

// ReSharper disable once UnusedTypeParameter
[PublicAPI]
public interface IIncludableDataQueryable<TEntity, TCollection, TProperty> : IIncludableDataQueryable<TEntity>
    where TEntity : class
    where TCollection : IEnumerable<TProperty>
{
    IIncludableDataQueryable<TEntity, TSubProperty> ThenInclude<TSubProperty>(Expression<Func<TProperty, TSubProperty>> includeSubProperty)
        where TSubProperty : class;

    IIncludableDataQueryable<TEntity, IEnumerable<TSubProperty>, TSubProperty> ThenIncludeCollection<TSubProperty>(
        Expression<Func<TProperty, IEnumerable<TSubProperty>>> includeSubPropertyCollection)
        where TSubProperty : class;
}