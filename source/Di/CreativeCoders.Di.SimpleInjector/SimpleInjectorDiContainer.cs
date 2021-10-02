using System;
using System.Collections.Generic;
using CreativeCoders.Core;
using CreativeCoders.Di.Exceptions;
using JetBrains.Annotations;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace CreativeCoders.Di.SimpleInjector
{
    [PublicAPI]
    public class SimpleInjectorDiContainer : DiContainerBase, IDiContainer
    {
        private readonly Container _container;

        private readonly Func<Container, Scope> _beginScope;

        public SimpleInjectorDiContainer(Container container) : this(container, AsyncScopedLifestyle.BeginScope) { }

        public SimpleInjectorDiContainer(Container container, Func<Container, Scope> beginScope)
        {
            Ensure.IsNotNull(container, nameof(container));
            Ensure.IsNotNull(beginScope, nameof(beginScope));

            _container = container;
            _beginScope = beginScope;
        }

        public object GetService(Type serviceType)
        {
            return GetInstance(serviceType);
        }

        public T GetInstance<T>()
            where T : class
        {
            return Resolve<T, ActivationException>(() => _container.GetInstance<T>());
        }

        public object GetInstance(Type serviceType)
        {
            return Resolve<ActivationException>(serviceType, () => _container.GetInstance(serviceType));
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
                return Resolve<KeyNotFoundException>(serviceType, () => serviceByNameFactory.GetServiceInstance(this, name));
            }
            throw new ResolveFailedException(serviceType, null);
        }

        public IEnumerable<T> GetInstances<T>()
            where T : class
        {
            return TryGetInstance<IEnumerable<T>>(out var instances)
                ? instances
                : Array.Empty<T>();
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
            try
            {
                return _container.GetAllInstances(serviceType);
            }
            catch (ActivationException)
            {
                return Array.Empty<object>();
            }
        }

        public bool TryGetInstance<T>(out T instance) where T : class
        {
            instance = (_container as IServiceProvider).GetService(typeof(T)) as T;
            return instance != null;
        }

        public bool TryGetInstance(Type serviceType, out object instance)
        {
            instance = (_container as IServiceProvider).GetService(serviceType);
            return instance != null;
        }

        public IDiContainerScope CreateScope()
        {
            var scope = _beginScope(_container);
            var containerScope = new DiContainerScope(new SimpleInjectorDiContainer(scope.Container, _beginScope), () => scope.Dispose());
            return containerScope;
        }
    }
}
