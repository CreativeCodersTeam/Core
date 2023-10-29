namespace CreativeCoders.Data.NoSql.LiteDb;

public interface ILiteDbRepositoryBuilder
{
    ILiteDbRepositoryBuilder AddCollection<T, TKey>(string? name = null)
        where T : class, IDocumentKey<TKey>;

    ILiteDbRepositoryBuilder AddCollection<T, TKey>(Action<ILiteCollectionIndexBuilder<T>> indexBuilder, string? name = null)
        where T : class, IDocumentKey<TKey>;
}
