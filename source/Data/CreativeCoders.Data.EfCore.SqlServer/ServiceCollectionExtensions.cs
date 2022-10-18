using CreativeCoders.Config.Base;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CreativeCoders.Data.EfCore.SqlServer;

[PublicAPI]
public static class ServiceCollectionExtensions
{
    public static void AddSqlServerDbContext<TDbContext>(this IServiceCollection services,
        string connectionString)
        where TDbContext : DbContext
    {
        services.AddEfCoreSupport<TDbContext>();

        services.AddScoped<ISetting<SqlServerConnectionString>>(_ =>
            new SqlServerConnectionStringSetting(connectionString));
    }
}
