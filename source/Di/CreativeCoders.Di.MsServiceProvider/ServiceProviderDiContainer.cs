using System;
using System.Collections.Generic;
using CreativeCoders.Core;
using CreativeCoders.Di.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace CreativeCoders.Di.MsServiceProvider;

public class ServiceProviderDiContainer : DiContainerBase, IDiContainer
{
    private IServiceProvider _serviceProvider;

    internal ServiceProviderDiContainer() { }

    public ServiceProviderDiContainer(IServiceProvider serviceProvider)
    {
        Ensure.IsNotNull(serviceProvider, nameof(serviceProvider));

        _serviceProvider = serviceProvider;
    }

    internal void SetServiceProvider(IServiceProvider serviceProvider)
    {
        Ensure.IsNotNull(serviceProvider, nameof(serviceProvider));

        _serviceProvider = serviceProvider;
    }

    public T GetInstance<T>()
        where T : class
    {
        return Resolve<T, InvalidOperationException>(() =>
            ActivatorUtilities.GetServiceOrCreateInstance<T>(_serviceProvider));
    }

    public object GetInstance(Type serviceType)
    {
        return Resolve<InvalidOperationException>(serviceType,
            () => ActivatorUtilities.GetServiceOrCreateInstance(_serviceProvider, serviceType));
    }

    public T GetInstance<T>(string name)
        where T : class
    {
        var factory = GetInstance<IServiceByNameFactory<T>>();
        var service = Resolve<T, KeyNotFoundException>(() => factory.GetInstance(this, name));

        return service;
    }

    public object GetInstance(Type serviceType, string name)
    {
        if (TryGetServiceByNameFactory(serviceType, out var serviceByNameFactory))
        {
            return Resolve<KeyNotFoundException>(serviceType,
                () => serviceByNameFactory.GetServiceInstance(this, name));
        }

        throw new ResolveFailedException(serviceType, null);
    }

    public IEnumerable<T> GetInstances<T>()
        where T : class
    {
        return _serviceProvider.GetServices<T>();
    }

    private bool TryGetServiceByNameFactory(Type serviceType, out IServiceByNameFactory serviceByNameFactory)
    {
        var factoryType = typeof(IServiceByNameFactory<>).MakeGenericType(serviceType);
        if (TryGetInstance(factoryType, out var factory))
        {
            serviceByNameFactory = factory as IServiceByNameFactory;
            return serviceByNameFactory != null;
        }

        serviceByNameFactory = null;
        return false;
    }

    public IEnumerable<object> GetInstances(Type serviceType)
    {
        return _serviceProvider.GetServices(serviceType);
    }

    public bool TryGetInstance<T>(out T instance) where T : class
    {
        instance = _serviceProvider.GetService(typeof(T)) as T;
        return instance != null;
    }

    public bool TryGetInstance(Type serviceType, out object instance)
    {
        instance = _serviceProvider.GetService(serviceType);
        return instance != null;
    }

    public IDiContainerScope CreateScope()
    {
        var scope = _serviceProvider.CreateScope();
        var containerScope = new DiContainerScope(new ServiceProviderDiContainer(scope.ServiceProvider),
            () => scope.Dispose());
        return containerScope;
    }

    public object GetService(Type serviceType)
    {
        return GetInstance(serviceType);
    }
}
