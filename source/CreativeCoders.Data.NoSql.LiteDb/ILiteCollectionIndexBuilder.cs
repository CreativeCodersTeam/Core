using System.Linq.Expressions;
using JetBrains.Annotations;

namespace CreativeCoders.Data.NoSql.LiteDb;

[PublicAPI]
public interface ILiteCollectionIndexBuilder<T>
{
    ILiteCollectionIndexBuilder<T> AddIndex<TProperty>(Expression<Func<T, TProperty>> field, bool unique = false);
}
