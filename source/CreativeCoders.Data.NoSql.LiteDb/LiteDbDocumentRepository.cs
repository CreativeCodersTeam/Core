using CreativeCoders.Core;
using LiteDB;

namespace CreativeCoders.Data.NoSql.LiteDb;

public class LiteDbDocumentRepository<T, TKey> : IDocumentRepository<T, TKey>
    where T : class, IDocumentKey<TKey>
{
    private readonly ILiteCollection<T> _liteDbCollection;

    public LiteDbDocumentRepository(ILiteCollection<T> liteDbCollection)
    {
        _liteDbCollection = Ensure.NotNull(liteDbCollection);
    }

    public Task<T> AddAsync(T entity)
    {
        Ensure.NotNull(entity);
        var id = _liteDbCollection.Insert(entity);

        entity.Id = (TKey) id.RawValue;

        return Task.FromResult(entity);
    }

    public Task DeleteAsync(TKey id)
    {
        Ensure.NotNull(id);

        _liteDbCollection.Delete(new BsonValue(id));
        return Task.CompletedTask;
    }

    public Task<T> GetAsync(TKey id)
    {
        Ensure.NotNull(id);

        return Task.FromResult(_liteDbCollection.FindOne(x => id.Equals(x.Id)));
    }

    public Task<IEnumerable<T>> GetAllAsync()
    {
        return Task.FromResult(_liteDbCollection.FindAll());
    }

    public Task UpdateAsync(T entity)
    {
        Ensure.NotNull(entity);

        if (entity.Id is null)
        {
            throw new InvalidOperationException("Entity has no id");
        }

        _liteDbCollection.Update(new BsonValue(entity.Id), entity);

        return Task.CompletedTask;
    }

    public Task ClearAsync()
    {
        _liteDbCollection.DeleteAll();

        return Task.CompletedTask;
    }
}
