using CreativeCoders.Config.Base;
using CreativeCoders.Data.EfCore.Modeling;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace CreativeCoders.Data.EfCore.SqlServer;

[PublicAPI]
public class EfCoreSqlServerDbContext : EfCoreDbContext
{
    public EfCoreSqlServerDbContext(ISetting<SqlServerConnectionString> settings,
        IEfCoreEntityModelBuilderSource entityModelBuilderSource) : base(entityModelBuilderSource)
    {
        ConnectionString = settings.Value.ConnectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(ConnectionString);
    }

    public string ConnectionString { get; set; }
}
