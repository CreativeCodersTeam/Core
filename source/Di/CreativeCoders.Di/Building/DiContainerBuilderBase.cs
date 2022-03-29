using System;
using CreativeCoders.Core;

namespace CreativeCoders.Di.Building;

public abstract class DiContainerBuilderBase : IDiContainerBuilder
{
    public abstract IDiContainerBuilder AddTransient(Type serviceType, Type implementationType);

    public virtual IDiContainerBuilder AddTransient(Type serviceType)
    {
        return AddTransient(serviceType, serviceType);
    }

    public virtual IDiContainerBuilder AddTransient<TService, TImplementation>() where TService : class
        where TImplementation : class, TService
    {
        return AddTransient(typeof(TService), typeof(TImplementation));
    }

    public abstract IDiContainerBuilder AddTransient<TService>(Func<IDiContainer, TService> implementationFactory)
        where TService : class;

    public virtual IDiContainerBuilder AddTransient<TService>() where TService : class
    {
        return AddTransient(typeof(TService));
    }

    public abstract IDiContainerBuilder AddScoped(Type serviceType, Type implementationType);

    public virtual IDiContainerBuilder AddScoped(Type serviceType)
    {
        return AddScoped(serviceType, serviceType);
    }

    public virtual IDiContainerBuilder AddScoped<TService, TImplementation>() where TService : class
        where TImplementation : class, TService
    {
        return AddScoped(typeof(TService), typeof(TImplementation));
    }

    public abstract IDiContainerBuilder AddScoped<TService>(Func<IDiContainer, TService> implementationFactory)
        where TService : class;

    public virtual IDiContainerBuilder AddScoped<TService>() where TService : class
    {
        return AddScoped(typeof(TService));
    }

    public abstract IDiContainerBuilder AddSingleton(Type serviceType, Type implementationType);

    public virtual IDiContainerBuilder AddSingleton(Type serviceType)
    {
        return AddSingleton(serviceType, serviceType);
    }

    public virtual IDiContainerBuilder AddSingleton<TService, TImplementation>() where TService : class
        where TImplementation : class, TService
    {
        return AddSingleton(typeof(TService), typeof(TImplementation));
    }

    public abstract IDiContainerBuilder AddSingleton<TService>(Func<IDiContainer, TService> implementationFactory)
        where TService : class;

    public virtual IDiContainerBuilder AddSingleton<TService>() where TService : class
    {
        return AddSingleton(typeof(TService));
    }

    public virtual IDiContainerBuilder AddTransientCollection<TService>(params Type[] implementationTypes)
        where TService : class
    {
        return AddTransientCollection(typeof(TService), implementationTypes);
    }

    public abstract IDiContainerBuilder AddTransientCollection<TService>(
        params Func<IDiContainer, TService>[] implementationFactories)
        where TService : class;

    public abstract IDiContainerBuilder AddTransientCollection(Type serviceType, params Type[] implementationTypes);

    public virtual IDiContainerBuilder AddScopedCollection<TService>(params Type[] implementationTypes)
        where TService : class
    {
        return AddScopedCollection(typeof(TService), implementationTypes);
    }

    public abstract IDiContainerBuilder AddScopedCollection<TService>(
        params Func<IDiContainer, TService>[] implementationFactories)
        where TService : class;

    public abstract IDiContainerBuilder AddScopedCollection(Type serviceType, params Type[] implementationTypes);

    public virtual IDiContainerBuilder AddSingletonCollection<TService>(params Type[] implementationTypes)
        where TService : class
    {
        return AddSingletonCollection(typeof(TService), implementationTypes);
    }

    public abstract IDiContainerBuilder AddSingletonCollection<TService>(
        params Func<IDiContainer, TService>[] implementationFactories)
        where TService : class;

    public abstract IDiContainerBuilder AddSingletonCollection(Type serviceType, params Type[] implementationTypes);

    public abstract INamedRegistrationBuilder<TService> AddTransientNamed<TService>()
        where TService : class;

    public abstract INamedRegistrationBuilder<TService> AddScopedNamed<TService>()
        where TService : class;

    public abstract INamedRegistrationBuilder<TService> AddSingletonNamed<TService>()
        where TService : class;

    public abstract IDiContainer Build();

    protected void RegisterDefault()
    {
        AddScoped(typeof(IClassFactory<>), typeof(DiContainerClassFactory<>));
        AddScoped<IClassFactory, DiContainerClassFactory>();
    }
}