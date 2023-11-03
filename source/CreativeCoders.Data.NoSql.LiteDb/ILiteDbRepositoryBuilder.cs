namespace CreativeCoders.Data.NoSql.LiteDb;

public interface ILiteDbRepositoryBuilder
{
    ILiteDbRepositoryBuilder AddRepository<T, TKey>(string? name = null)
        where T : class, IDocumentKey<TKey>
        where TKey : IEquatable<TKey>;

    ILiteDbRepositoryBuilder AddRepository<T, TKey>(Action<ILiteCollectionIndexBuilder<T>> indexBuilder, string? name = null)
        where T : class, IDocumentKey<TKey>
        where TKey : IEquatable<TKey>;
}
