using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Builder;
using Autofac.Features.ResolveAnything;
using CreativeCoders.Core;
using CreativeCoders.Di.Building;

namespace CreativeCoders.Di.Autofac
{
    public class AutofacDiContainerBuilder : DiContainerBuilderBase
    {
        private readonly ContainerBuilder _containerBuilder;

        public AutofacDiContainerBuilder(ContainerBuilder containerBuilder)
        {
            Ensure.IsNotNull(containerBuilder, nameof(containerBuilder));
            
            _containerBuilder = containerBuilder;
        }

        public override IDiContainerBuilder AddTransient(Type serviceType, Type implementationType)
        {
            if (serviceType.IsGenericTypeDefinition)
            {
                _containerBuilder
                    .RegisterGeneric(implementationType)
                    .As(serviceType)
                    .InstancePerDependency();
            }
            else
            {
                _containerBuilder
                    .RegisterType(implementationType)
                    .As(serviceType)
                    .InstancePerDependency();
            }
            
            return this;
        }

        public override IDiContainerBuilder AddTransient<TService>(Func<IDiContainer, TService> implementationFactory)
        {
            _containerBuilder
                .Register((c, _) => implementationFactory(c.Resolve<IDiContainer>()))
                .As<TService>()
                .InstancePerDependency();

            return this;
        }

        public override IDiContainerBuilder AddScoped(Type serviceType, Type implementationType)
        {
            if (serviceType.IsGenericTypeDefinition)
            {
                _containerBuilder
                    .RegisterGeneric(implementationType)
                    .As(serviceType)
                    .InstancePerLifetimeScope();
            }
            else
            {
                _containerBuilder
                    .RegisterType(implementationType)
                    .As(serviceType)
                    .InstancePerLifetimeScope();
            }

            return this;
        }

        public override IDiContainerBuilder AddScoped<TService>(Func<IDiContainer, TService> implementationFactory)
        {
            _containerBuilder
                .Register((c, _) => implementationFactory(c.Resolve<IDiContainer>()))
                .As<TService>()
                .InstancePerLifetimeScope();

            return this;
        }

        public override IDiContainerBuilder AddSingleton(Type serviceType, Type implementationType)
        {
            if (serviceType.IsGenericTypeDefinition)
            {
                _containerBuilder
                    .RegisterGeneric(implementationType)
                    .As(serviceType)
                    .SingleInstance();
            }
            else
            {
                _containerBuilder
                    .RegisterType(implementationType)
                    .As(serviceType)
                    .SingleInstance();
            }

            return this;
        }

        public override IDiContainerBuilder AddSingleton<TService>(Func<IDiContainer, TService> implementationFactory)
        {
            _containerBuilder
                .Register((c, _) => implementationFactory(c.Resolve<IDiContainer>()))
                .As<TService>()
                .SingleInstance();

            return this;
        }

        public override IDiContainerBuilder AddTransientCollection<TService>(params Func<IDiContainer, TService>[] implementationFactories)
        {
            implementationFactories
                .ForEach(implementationFactory =>
                    AddTransient(implementationFactory));

            return this;
        }

        public override IDiContainerBuilder AddTransientCollection(Type serviceType, params Type[] implementationTypes)
        {
            implementationTypes
                .ForEach(implementationType =>
                    AddTransient(serviceType, implementationType));

            return this;
        }

        public override IDiContainerBuilder AddScopedCollection<TService>(params Func<IDiContainer, TService>[] implementationFactories)
        {
            implementationFactories
                .ForEach(implementationFactory =>
                    AddScoped(implementationFactory));

            return this;
        }

        public override IDiContainerBuilder AddScopedCollection(Type serviceType, params Type[] implementationTypes)
        {
            implementationTypes
                .ForEach(implementationType =>
                    AddScoped(serviceType, implementationType));

            return this;
        }

        public override IDiContainerBuilder AddSingletonCollection<TService>(params Func<IDiContainer, TService>[] implementationFactories)
        {
            implementationFactories
                .ForEach(implementationFactory =>
                    AddSingleton(implementationFactory));

            return this;
        }

        public override IDiContainerBuilder AddSingletonCollection(Type serviceType, params Type[] implementationTypes)
        {
            implementationTypes
                .ForEach(implementationType => 
                    AddSingleton(serviceType, implementationType));

            return this;
        }

        public override INamedRegistrationBuilder<TService> AddTransientNamed<TService>()
        {
            return new NamedRegistrationBuilder<TService>(nameMap =>
                AddNamed<TService>(nameMap, reg => reg.InstancePerDependency()));
        }

        public override INamedRegistrationBuilder<TService> AddScopedNamed<TService>()
        {
            return new NamedRegistrationBuilder<TService>(nameMap =>
                AddNamed<TService>(nameMap, reg => reg.InstancePerLifetimeScope()));
        }

        public override INamedRegistrationBuilder<TService> AddSingletonNamed<TService>()
        {
            return new NamedRegistrationBuilder<TService>(nameMap =>
                AddNamed<TService>(nameMap, reg => reg.SingleInstance()));
        }

        private void AddNamed<TService>(IDictionary<string, Type> nameMap,
            Action<IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>> setupLifetime)
            where TService : class
        {
            nameMap
                .ForEach(named =>
                    setupLifetime(_containerBuilder
                        .RegisterType(named.Value)
                        .Named<TService>(named.Key)));
        }

        public override IDiContainer Build()
        {
            AddScoped<IDiContainer, AutofacDiContainer>();
            RegisterDefault();
            _containerBuilder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());

            var container = _containerBuilder.Build() as ILifetimeScope;

            var diContainer = container.Resolve<IDiContainer>();

            return diContainer;
        }
    }
}
