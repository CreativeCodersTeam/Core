using System.Linq.Expressions;
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

    public Task<TKey> AddAsync(T entity)
    {
        Ensure.NotNull(entity);
        var id = _liteDbCollection.Insert(entity);

        var key = (TKey)id.RawValue;
        entity.Id = key;

        return Task.FromResult(key);
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

        //return Task.FromResult(_liteDbCollection.Query().Where(x => id.Equals(x.Id)).FirstOrDefault());
        return Task.FromResult(_liteDbCollection.FindOne(x => id.Equals(x.Id)));
    }

    public Task<IEnumerable<T>> GetAllAsync()
    {
        return Task.FromResult(_liteDbCollection.FindAll());
    }

    public Task<IEnumerable<T>> QueryAsync(Expression<Func<T, bool>> query)
    {
        return Task.FromResult(_liteDbCollection.Query().Where(query).ToEnumerable());
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
