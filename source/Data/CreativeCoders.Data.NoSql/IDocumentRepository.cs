using System.Linq.Expressions;

namespace CreativeCoders.Data.NoSql;

public interface IDocumentRepository<T, TKey>
    where T : class, IDocumentKey<TKey>
{
    Task<TKey> AddAsync(T entity);

    Task DeleteAsync(TKey id);

    Task DeleteAsync(Expression<Func<T, bool>> predicate);

    Task<T?> GetAsync(TKey id);

    Task<IEnumerable<T>> GetAllAsync();

    Task<IEnumerable<T>> QueryAsync(Expression<Func<T, bool>> query);

    Task UpdateAsync(T entity);

    Task ClearAsync();
}
