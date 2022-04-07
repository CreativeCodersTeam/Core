using System;
using CreativeCoders.Di.Building;
using JetBrains.Annotations;

namespace CreativeCoders.Data;

[PublicAPI]
public static class ContainerBuilderExtensions
{
    public static IDiContainerBuilder AddDataSupport(this IDiContainerBuilder builder, Type repositoryType,
        Type repositoryWithKeyType)
    {
        builder.AddScoped<IRepositoryFactory, RepositoryFactory>();
        builder.AddScoped(typeof(IRepository<,>), repositoryWithKeyType);
        builder.AddScoped(typeof(IRepository<>), repositoryType);

        return builder;
    }
}
