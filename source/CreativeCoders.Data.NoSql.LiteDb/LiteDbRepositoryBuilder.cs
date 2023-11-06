using CreativeCoders.Core;
using LiteDB;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CreativeCoders.Data.NoSql.LiteDb;

public class LiteDbRepositoryBuilder : ILiteDbRepositoryBuilder
{
    private readonly IServiceCollection _services;

    public LiteDbRepositoryBuilder(IServiceCollection services)
    {
        _services = Ensure.NotNull(services);
    }

    public ILiteDbRepositoryBuilder AddRepository<T, TKey>(string? name = null)
        where T : class, IDocumentKey<TKey>
        where TKey : IEquatable<TKey>
    {
        return AddRepository<T, TKey>(_ => { }, name);
    }

    public ILiteDbRepositoryBuilder AddRepository<T, TKey>(Action<ILiteCollectionIndexBuilder<T>> buildIndex, string? name = null)
        where T : class, IDocumentKey<TKey>
        where TKey : IEquatable<TKey>
    {
        _services.TryAddScoped<IDocumentRepository<T, TKey>, LiteDbDocumentRepository<T, TKey>>();

        _services.TryAddScoped<ILiteCollection<T>>(sp =>
        {
            var liteDb = sp.GetRequiredService<ILiteDatabase>();

            var collection =  liteDb.GetCollection<T>(name ?? typeof(T).Name);

            var indexBuilder = new LiteCollectionIndexBuilder<T>(collection);

            buildIndex(indexBuilder);

            return collection;
        });

        return this;
    }
}
