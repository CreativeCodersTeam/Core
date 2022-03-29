using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Core.Registration;
using CreativeCoders.Core;
using JetBrains.Annotations;

namespace CreativeCoders.Di.Autofac;

[PublicAPI]
public class AutofacDiContainer : DiContainerBase, IDiContainer
{
    private readonly ILifetimeScope _container;

    public AutofacDiContainer(ILifetimeScope container)
    {
        Ensure.IsNotNull(container, nameof(container));

        _container = container;
    }

    public object GetService(Type serviceType)
    {
        return GetInstance(serviceType);
    }

    public T GetInstance<T>()
        where T : class
    {
        return Resolve<T, ComponentNotRegisteredException>(() => _container.Resolve<T>());
    }

    public object GetInstance(Type serviceType)
    {
        return Resolve<ComponentNotRegisteredException>(serviceType, () => _container.Resolve(serviceType));
    }

    public T GetInstance<T>(string name) where T : class
    {
        return Resolve<T, ComponentNotRegisteredException>(() => _container.ResolveNamed<T>(name));
    }

    public object GetInstance(Type serviceType, string name)
    {
        return Resolve<ComponentNotRegisteredException>(serviceType,
            () => _container.ResolveNamed(name, serviceType));
    }

    public IEnumerable<T> GetInstances<T>() where T : class
    {
        return _container.Resolve<IEnumerable<T>>();
    }

    public IEnumerable<object> GetInstances(Type serviceType)
    {
        var enumerableType = typeof(IEnumerable<>).MakeGenericType(serviceType);
        return _container.Resolve(enumerableType) as IEnumerable<object>;
    }

    public bool TryGetInstance<T>(out T instance) where T : class
    {
        return _container.TryResolve(out instance);
    }

    public bool TryGetInstance(Type serviceType, out object instance)
    {
        return _container.TryResolve(serviceType, out instance);
    }

    public IDiContainerScope CreateScope()
    {
        var lifetimeScope = _container.BeginLifetimeScope();
        var containerScope =
            new DiContainerScope(new AutofacDiContainer(lifetimeScope), () => lifetimeScope.Dispose());
        return containerScope;
    }
}
