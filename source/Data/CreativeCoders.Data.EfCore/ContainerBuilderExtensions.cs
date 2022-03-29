using CreativeCoders.Di.Building;
using CreativeCoders.Data.EfCore.Modeling;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace CreativeCoders.Data.EfCore;

[PublicAPI]
public static class ContainerBuilderExtensions
{
    public static IDiContainerBuilder AddEfCoreSupport<TDbContext>(this IDiContainerBuilder builder)
        where TDbContext : DbContext
    {
        builder.AddDataSupport(typeof(EfCoreRepository<>), typeof(EfCoreRepository<,>));

        builder.AddScoped<DbContext, TDbContext>();
        builder.AddScoped<IEfCoreEntityModelBuilderSource, EfCoreEntityModelBuilderSource>();

        return builder;
    }
}