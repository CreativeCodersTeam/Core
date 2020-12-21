using CreativeCoders.Config.Base;
using CreativeCoders.Di.Building;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace CreativeCoders.Data.EfCore.SqlServer
{
    [PublicAPI]
    public static class ContainerBuilderExtensions
    {
        public static IDiContainerBuilder AddSqlServerDbContext<TDbContext>(this IDiContainerBuilder builder,
            string connectionString)
            where TDbContext : DbContext
        {
            builder.AddEfCoreSupport<TDbContext>();

            builder.AddScoped<ISetting<SqlServerConnectionString>>(_ =>
                new SqlServerConnectionStringSetting(connectionString));

            return builder;
        }
    }
}
