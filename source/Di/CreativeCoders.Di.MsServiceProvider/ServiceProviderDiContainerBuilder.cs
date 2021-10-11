using System;
using System.Collections.Generic;
using CreativeCoders.Core;
using CreativeCoders.Core.Collections;
using CreativeCoders.Di.Building;
using Microsoft.Extensions.DependencyInjection;

namespace CreativeCoders.Di.MsServiceProvider
{
    public class ServiceProviderDiContainerBuilder : DiContainerBuilderBase
    {
        private readonly IServiceCollection _services;

        public ServiceProviderDiContainerBuilder(IServiceCollection services)
        {
            Ensure.IsNotNull(services, nameof(services));
            
            _services = services;
        }

        public override IDiContainerBuilder AddTransient(Type serviceType, Type implementationType)
        {
            _services.AddTransient(serviceType, implementationType);
            return this;
        }

        public override IDiContainerBuilder AddTransient<TService>(Func<IDiContainer, TService> implementationFactory)
        {
            _services.AddTransient(sp => implementationFactory(sp.GetRequiredService<IDiContainer>()));
            return this;
        }

        public override IDiContainerBuilder AddScoped(Type serviceType, Type implementationType)
        {
            _services.AddScoped(serviceType, implementationType);
            return this;
        }

        public override IDiContainerBuilder AddScoped<TService>(Func<IDiContainer, TService> implementationFactory)
        {
            _services.AddScoped(sp =>
                implementationFactory(sp.GetRequiredService<IDiContainer>()));
            return this;
        }

        public override IDiContainerBuilder AddSingleton(Type serviceType, Type implementationType)
        {
            _services.AddSingleton(serviceType, implementationType);
            return this;
        }

        public override IDiContainerBuilder AddSingleton<TService>(Func<IDiContainer, TService> implementationFactory)
        {
            _services.AddSingleton(sp => implementationFactory(sp.GetRequiredService<IDiContainer>()));
            return this;
        }

        public override IDiContainerBuilder AddTransientCollection<TService>(params Func<IDiContainer, TService>[] implementationFactories)
        {
            implementationFactories.ForEach(implementationFactory => AddTransient(implementationFactory));

            return this;
        }

        public override IDiContainerBuilder AddTransientCollection(Type serviceType, params Type[] implementationTypes)
        {
            implementationTypes.ForEach(implementationType => AddTransient(serviceType, implementationType));

            return this;
        }

        public override IDiContainerBuilder AddScopedCollection<TService>(params Func<IDiContainer, TService>[] implementationFactories)
        {
            implementationFactories.ForEach(implementationFactory => AddScoped(implementationFactory));

            return this;
        }

        public override IDiContainerBuilder AddScopedCollection(Type serviceType, params Type[] implementationTypes)
        {
            implementationTypes.ForEach(implementationType => AddScoped(serviceType, implementationType));

            return this;
        }

        public override IDiContainerBuilder AddSingletonCollection<TService>(params Func<IDiContainer, TService>[] implementationFactories)
        {
            implementationFactories.ForEach(implementationFactory => AddSingleton(implementationFactory));

            return this;
        }

        public override IDiContainerBuilder AddSingletonCollection(Type serviceType, params Type[] implementationTypes)
        {
            implementationTypes.ForEach(implementationType => AddSingleton(serviceType, implementationType));

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
            var diContainer = new ServiceProviderDiContainer();
            AddScoped<IDiContainer, ServiceProviderDiContainer>();
            RegisterDefault();
            
            var serviceProvider = _services.BuildServiceProvider() as IServiceProvider;
            diContainer.SetServiceProvider(serviceProvider);

            return diContainer;
        }
    }
}
