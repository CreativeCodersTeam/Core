namespace CreativeCoders.Data.NoSql;

public interface IDocumentRepository<T, in TKey>
    where T : class, IDocumentKey<TKey>
{
    Task<T> AddAsync(T entity);

    Task DeleteAsync(TKey id);

    Task<T> GetAsync(TKey id);

    Task<IEnumerable<T>> GetAllAsync();

    Task UpdateAsync(T entity);

    Task ClearAsync();
}
