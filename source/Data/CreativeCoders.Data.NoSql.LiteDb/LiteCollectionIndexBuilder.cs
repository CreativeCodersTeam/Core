using System.Linq.Expressions;
using CreativeCoders.Core;
using LiteDB;

namespace CreativeCoders.Data.NoSql.LiteDb;

public class LiteCollectionIndexBuilder<T> : ILiteCollectionIndexBuilder<T>
{
    private readonly ILiteCollection<T> _collection;

    public LiteCollectionIndexBuilder(ILiteCollection<T> collection)
    {
        _collection = Ensure.NotNull(collection);
    }

    public ILiteCollectionIndexBuilder<T> AddIndex<TProperty>(Expression<Func<T, TProperty>> field, bool unique = false)
    {
        _collection.EnsureIndex(field, unique);

        return this;
    }
}
