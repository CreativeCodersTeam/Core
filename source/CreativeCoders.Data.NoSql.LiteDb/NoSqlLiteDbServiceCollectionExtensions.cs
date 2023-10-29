using LiteDB;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CreativeCoders.Data.NoSql.LiteDb;

public static class NoSqlLiteDbServiceCollectionExtensions
{
    public static ILiteDbRepositoryBuilder AddLiteDbDocumentRepository(
        this IServiceCollection services, string dbConnectionString)
    {
        services.TryAddSingleton<ILiteDatabase>(_ => new LiteDatabase(dbConnectionString));

        return new LiteDbRepositoryBuilder(services);
    }
}
