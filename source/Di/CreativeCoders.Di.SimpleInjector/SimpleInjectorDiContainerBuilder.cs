using System;
using System.Collections.Generic;
using System.Linq;
using CreativeCoders.Core;
using CreativeCoders.Di.Building;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace CreativeCoders.Di.SimpleInjector
{
    public class SimpleInjectorDiContainerBuilder : DiContainerBuilderBase
    {
        private readonly Container _container;

        private readonly Lifestyle _scopedLifestyle;

        private bool _verifyOnBuild;

        public SimpleInjectorDiContainerBuilder(Container container) : this(container,
            AsyncScopedLifestyle.BeginScope) { }

        public SimpleInjectorDiContainerBuilder(Container container, Func<Container, Scope> beginScope)
        {
            Ensure.IsNotNull(container, nameof(container));
            Ensure.IsNotNull(beginScope, nameof(beginScope));

            _container = container;
            
            _scopedLifestyle = Lifestyle.CreateHybrid(new AsyncScopedLifestyle(), Lifestyle.Singleton);

            if (_container.Options.DefaultScopedLifestyle == null && _container.GetCurrentRegistrations().Length == 0)
            {
                _container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
            }
        }

        public SimpleInjectorDiContainerBuilder WithVerify()
        {
            _verifyOnBuild = true;
            return this;
        }

        public override IDiContainerBuilder AddTransient(Type serviceType, Type implementationType)
        {
            _container.Register(serviceType, implementationType);
            return this;
        }

        public override IDiContainerBuilder AddTransient<TService>(Func<IDiContainer, TService> implementationFactory)
        {
            _container.Register(() => implementationFactory(_container.GetInstance<IDiContainer>()));
            return this;
        }

        public override IDiContainerBuilder AddScoped(Type serviceType, Type implementationType)
        {
            _container.Register(serviceType, implementationType, _scopedLifestyle);
            return this;
        }

        public override IDiContainerBuilder AddScoped<TService>(Func<IDiContainer, TService> implementationFactory)
        {
            _container.Register(() => implementationFactory(_container.GetInstance<IDiContainer>()), _scopedLifestyle);
            return this;
        }

        public override IDiContainerBuilder AddSingleton(Type serviceType, Type implementationType)
        {
            _container.RegisterSingleton(serviceType, implementationType);
            return this;
        }

        public override IDiContainerBuilder AddSingleton<TService>(Func<IDiContainer, TService> implementationFactory)
        {
            _container.RegisterSingleton(() => implementationFactory(_container.GetInstance<IDiContainer>()));
            return this;
        }

        public override IDiContainerBuilder AddTransientCollection<TService>(params Func<IDiContainer, TService>[] implementationFactories)
        {
            var registrations = implementationFactories.Select(implementationFactory =>
                Lifestyle.Transient.CreateRegistration(typeof(TService),
                    () => implementationFactory(_container.GetInstance<IDiContainer>()), _container));
            _container.Collection.Register<TService>(registrations);

            return this;
        }

        public override IDiContainerBuilder AddTransientCollection(Type serviceType, params Type[] implementationTypes)
        {
            _container.Collection.Register(serviceType, implementationTypes);
            return this;
        }

        public override IDiContainerBuilder AddScopedCollection<TService>(params Func<IDiContainer, TService>[] implementationFactories)
        {
            var registrations = implementationFactories.Select(implementationFactory =>
                _scopedLifestyle.CreateRegistration(typeof(TService),
                    () => implementationFactory(_container.GetInstance<IDiContainer>()), _container));
            _container.Collection.Register<TService>(registrations);

            return this;
        }

        public override IDiContainerBuilder AddScopedCollection(Type serviceType, params Type[] implementationTypes)
        {
            var registrations = implementationTypes.Select(implementationType =>
                _scopedLifestyle.CreateRegistration(implementationType, _container));
            _container.Collection.Register(serviceType, registrations);

            return this;
        }

        public override IDiContainerBuilder AddSingletonCollection<TService>(params Func<IDiContainer, TService>[] implementationFactories)
        {
            var registrations = implementationFactories.Select(implementationFactory =>
                Lifestyle.Singleton.CreateRegistration(typeof(TService),
                    () => implementationFactory(_container.GetInstance<IDiContainer>()), _container));
            _container.Collection.Register<TService>(registrations);

            return this;
        }

        public override IDiContainerBuilder AddSingletonCollection(Type serviceType, params Type[] implementationTypes)
        {
            var registrations = implementationTypes.Select(implementationType =>
                Lifestyle.Singleton.CreateRegistration(implementationType, _container));
            _container.Collection.Register(serviceType, registrations);
            
            return this;
        }

        public override INamedRegistrationBuilder<TService> AddTransientNamed<TService>()
        {
            return new NamedRegistrationBuilder<TService>(nameMap => AddNamed<TService>(nameMap,
                factory => AddTransient(factory), implementationType => AddTransient(implementationType)));
        }

        public override INamedRegistrationBuilder<TService> AddScopedNamed<TService>()
        {
            return new NamedRegistrationBuilder<TService>(nameMap => AddNamed<TService>(nameMap,
                factory => AddScoped(factory), implementationType => AddScoped(implementationType)));
        }

        public override INamedRegistrationBuilder<TService> AddSingletonNamed<TService>()
        {
            return new NamedRegistrationBuilder<TService>(nameMap => AddNamed<TService>(nameMap,
                factory => AddSingleton(factory), implementationType => AddSingleton(implementationType)));
        }

        // ReSharper disable once MemberCanBeMadeStatic.Local
        private void AddNamed<TService>(IDictionary<string, Type> nameMap,
            Action<Func<IDiContainer, IServiceByNameFactory<TService>>> addFactory, Action<Type> addImplementation)
            where TService : class
        {
            nameMap.Values.ForEach(addImplementation);
            addFactory(_ => new ServiceByNameFactory<TService>(nameMap));
        }

        public override IDiContainer Build()
        {
            _container.Options.ResolveUnregisteredConcreteTypes = true;

            _container.Register<IDiContainer, SimpleInjectorDiContainerForRegistration>(
                _scopedLifestyle);
            RegisterDefault();
            
            if (_verifyOnBuild)
            {
                _container.Verify();
            }

            var diContainer = _container.GetInstance<IDiContainer>();

            return diContainer;
        }
    }
}
