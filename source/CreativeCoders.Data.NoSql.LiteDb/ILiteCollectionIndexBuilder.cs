using System.Linq.Expressions;

namespace CreativeCoders.Data.NoSql.LiteDb;

public interface ILiteCollectionIndexBuilder<T>
{
    ILiteCollectionIndexBuilder<T> AddIndex<TProperty>(Expression<Func<T, TProperty>> field, bool unique = false);
}
