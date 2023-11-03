using CreativeCoders.Core;
using LiteDB;
using Microsoft.Extensions.DependencyInjection;

namespace CreativeCoders.Data.NoSql.LiteDb;

public class LiteDbRepositoryBuilder : ILiteDbRepositoryBuilder
{
    private readonly IServiceCollection _services;

    public LiteDbRepositoryBuilder(IServiceCollection services)
    {
        _services = Ensure.NotNull(services);
    }

    public ILiteDbRepositoryBuilder AddRepository<T, TKey>(string? name = null) where T : class, IDocumentKey<TKey>
    {
        return AddRepository<T, TKey>(_ => { }, name);
    }

    public ILiteDbRepositoryBuilder AddRepository<T, TKey>(Action<ILiteCollectionIndexBuilder<T>> buildIndex, string? name = null)
        where T : class, IDocumentKey<TKey>
    {
        _services.AddTransient<IDocumentRepository<T, TKey>, LiteDbDocumentRepository<T, TKey>>();

        _services.AddTransient<ILiteCollection<T>>(sp =>
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
