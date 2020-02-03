using System;
using JetBrains.Annotations;

namespace CreativeCoders.Di.Building
{
    [PublicAPI]
    public interface IDiContainerBuilder
    {
        IDiContainerBuilder AddTransient(Type serviceType, Type implementationType);

        IDiContainerBuilder AddTransient(Type serviceType);

        IDiContainerBuilder AddTransient<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService;

        IDiContainerBuilder AddTransient<TService>(Func<IDiContainer, TService> implementationFactory)
            where TService : class;

        IDiContainerBuilder AddTransient<TService>()
            where TService : class;

        IDiContainerBuilder AddScoped(Type serviceType, Type implementationType);

        IDiContainerBuilder AddScoped(Type serviceType);

        IDiContainerBuilder AddScoped<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService;

        IDiContainerBuilder AddScoped<TService>(Func<IDiContainer, TService> implementationFactory)
            where TService : class;

        IDiContainerBuilder AddScoped<TService>()
            where TService : class;

        IDiContainerBuilder AddSingleton(Type serviceType, Type implementationType);

        IDiContainerBuilder AddSingleton(Type serviceType);

        IDiContainerBuilder AddSingleton<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService;

        IDiContainerBuilder AddSingleton<TService>(Func<IDiContainer, TService> implementationFactory)
            where TService : class;

        IDiContainerBuilder AddSingleton<TService>()
            where TService : class;

        IDiContainerBuilder AddTransientCollection<TService>(params Type[] implementationTypes)
            where TService : class;

        IDiContainerBuilder AddTransientCollection<TService>(params Func<IDiContainer, TService>[] implementationFactories)
            where TService : class;

        IDiContainerBuilder AddTransientCollection(Type serviceType, params Type[] implementationTypes);

        IDiContainerBuilder AddScopedCollection<TService>(params Type[] implementationTypes)
            where TService : class;

        IDiContainerBuilder AddScopedCollection<TService>(params Func<IDiContainer, TService>[] implementationFactories)
            where TService : class;

        IDiContainerBuilder AddScopedCollection(Type serviceType, params Type[] implementationTypes);

        IDiContainerBuilder AddSingletonCollection<TService>(params Type[] implementationTypes)
            where TService : class;

        IDiContainerBuilder AddSingletonCollection<TService>(params Func<IDiContainer, TService>[] implementationFactories)
            where TService : class;

        IDiContainerBuilder AddSingletonCollection(Type serviceType, params Type[] implementationTypes);

        INamedRegistrationBuilder<TService> AddTransientNamed<TService>()
            where TService : class;

        INamedRegistrationBuilder<TService> AddScopedNamed<TService>()
            where TService : class;

        INamedRegistrationBuilder<TService> AddSingletonNamed<TService>()
            where TService : class;

        IDiContainer Build();
    }
}