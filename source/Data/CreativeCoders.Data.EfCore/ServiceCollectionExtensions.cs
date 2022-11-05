using CreativeCoders.Data.EfCore.Modeling;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CreativeCoders.Data.EfCore;

[PublicAPI]
public static class ServiceCollectionExtensions
{
    public static void AddEfCoreSupport<TDbContext>(this IServiceCollection services)
        where TDbContext : DbContext
    {
        services.TryAddScoped(typeof(IRepository<>), typeof(EfCoreRepository<>));
        services.TryAddScoped(typeof(IRepository<,>), typeof(EfCoreRepository<,>));

        services.TryAddScoped<DbContext, TDbContext>();
        services.TryAddScoped<IEfCoreEntityModelBuilderSource, EfCoreEntityModelBuilderSource>();
    }
}
