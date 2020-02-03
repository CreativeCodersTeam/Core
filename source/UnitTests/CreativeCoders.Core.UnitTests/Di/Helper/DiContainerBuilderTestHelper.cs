using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CreativeCoders.Di;
using CreativeCoders.Di.Building;
using CreativeCoders.Di.Exceptions;

namespace CreativeCoders.Core.UnitTests.Di.Helper
{
    [SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Global")]
    [SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Local")]
    public class DiContainerBuilderTestHelper
    {
        public void TestAddTransientCollection(Func<IDiContainerBuilder> createBuilderFunc)
        {
            var implementationTypes = new[] {typeof(TestService2), typeof(TestServiceWithNoCtorParam)};

            TestCollectionRegistration(createBuilderFunc,
                builder => builder.AddTransientCollection<ITestService>(implementationTypes),
                CheckTransientServices<ITestService>,
                implementationTypes);

            TestCollectionRegistration(createBuilderFunc,
                builder => builder.AddTransientCollection(typeof(ITestService), implementationTypes),
                CheckTransientServices<ITestService>,
                implementationTypes);

            var implementationFactories = new Func<IDiContainer, ITestService>[]
                {c => new TestService2(), c => new TestServiceWithNoCtorParam()};

            TestCollectionRegistration(createBuilderFunc,
                builder => builder.AddTransientCollection(implementationFactories),
                CheckTransientServices<ITestService>,
                implementationTypes);
        }

        public void TestAddScopedCollection(Func<IDiContainerBuilder> createBuilderFunc)
        {
            var implementationTypes = new[] {typeof(TestService2), typeof(TestServiceWithNoCtorParam)};

            TestCollectionRegistration(createBuilderFunc,
                builder => builder.AddScopedCollection<ITestService>(implementationTypes),
                CheckScopedServices<ITestService>,
                implementationTypes);

            TestCollectionRegistration(createBuilderFunc,
                builder => builder.AddScopedCollection(typeof(ITestService), implementationTypes),
                CheckScopedServices<ITestService>,
                implementationTypes);

            var implementationFactories = new Func<IDiContainer, ITestService>[]
                {c => new TestService2(), c => new TestServiceWithNoCtorParam()};

            TestCollectionRegistration(createBuilderFunc,
                builder => builder.AddScopedCollection(implementationFactories),
                CheckScopedServices<ITestService>,
                implementationTypes);
        }

        public void TestAddSingletonCollection(Func<IDiContainerBuilder> createBuilderFunc)
        {
            var implementationTypes = new[] {typeof(TestService2), typeof(TestServiceWithNoCtorParam)};

            TestCollectionRegistration(createBuilderFunc,
                builder => builder.AddSingletonCollection<ITestService>(implementationTypes),
                CheckSingletonServices<ITestService>,
                implementationTypes);

            TestCollectionRegistration(createBuilderFunc,
                builder => builder.AddSingletonCollection(typeof(ITestService), implementationTypes),
                CheckSingletonServices<ITestService>,
                implementationTypes);

            var implementationFactories = new Func<IDiContainer, ITestService>[]
                {c => new TestService2(), c => new TestServiceWithNoCtorParam()};

            TestCollectionRegistration(createBuilderFunc,
                builder => builder.AddSingletonCollection(implementationFactories),
                CheckSingletonServices<ITestService>,
                implementationTypes);
        }

        private void TestCollectionRegistration(Func<IDiContainerBuilder> createBuilderFunc, Action<IDiContainerBuilder> addRegistration, Action<IDiContainer, Type[]> checkServices, Type[] implementationTypes)
        {
            var builder = createBuilderFunc();

            addRegistration(builder);

            var container = builder.Build();

            checkServices(container, implementationTypes);
        }

        private void CheckTransientServices<TService>(IDiContainer container, Type[] implementationTypes)
            where TService : class
        {
            var services = container.GetInstances<TService>().ToArray();

            CheckServices(services, implementationTypes);

            var serviceObjects = container.GetInstances<TService>().ToArray();

            CheckServices(serviceObjects, implementationTypes);

            Xunit.Assert.True(services.All(service => !serviceObjects.ToList().Contains(service)));
        }

        private void CheckScopedServices<TService>(IDiContainer container, Type[] implementationTypes)
            where TService : class
        {
            var services = container.GetInstances<TService>().ToArray();

            CheckServices(services, implementationTypes);

            var serviceObjects = container.GetInstances<TService>().ToArray();

            CheckServices(serviceObjects, implementationTypes);

            Xunit.Assert.True(services.SequenceEqual(serviceObjects));
        }

        private void CheckSingletonServices<TService>(IDiContainer container, Type[] implementationTypes)
            where TService : class
        {
            var services = container.GetInstances<TService>().ToArray();

            CheckServices(services, implementationTypes);

            var serviceObjects = container.GetInstances<TService>().ToArray();

            CheckServices(serviceObjects, implementationTypes);

            Xunit.Assert.True(services.SequenceEqual(serviceObjects));
        }

        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        private static void CheckServices<TService>(IReadOnlyCollection<TService> services, Type[] implementationTypes)
        {
            Xunit.Assert.Equal(services.Count, implementationTypes.Length);

            foreach (var service in services)
            {
                Xunit.Assert.Contains(service.GetType(), implementationTypes);
            }
        }

        public void TestAddTransient(Func<IDiContainerBuilder> createBuilderFunc)
        {
            TestRegistration(createBuilderFunc,
                builder => builder.AddTransient<ITestService, TestServiceWithNoCtorParam>(),
                CheckTransientService<ITestService, TestServiceWithNoCtorParam>);
            TestRegistration(createBuilderFunc,
                builder => builder.AddTransient<TestServiceWithNoCtorParam>(), CheckTransientService<TestServiceWithNoCtorParam, TestServiceWithNoCtorParam>);
            TestRegistration(createBuilderFunc,
                builder => builder.AddTransient(typeof(TestServiceWithNoCtorParam)), CheckTransientService<TestServiceWithNoCtorParam, TestServiceWithNoCtorParam>);
            TestRegistration(createBuilderFunc,
                builder => builder.AddTransient(typeof(ITestService), typeof(TestServiceWithNoCtorParam)),
                CheckTransientService<ITestService, TestServiceWithNoCtorParam>);
            TestRegistration(createBuilderFunc,
                builder => builder.AddTransient<ITestService>(c => new TestServiceWithNoCtorParam()),
                CheckTransientService<ITestService, TestServiceWithNoCtorParam>);
        }

        public void TestAddSingleton(Func<IDiContainerBuilder> createBuilderFunc)
        {
            TestRegistration(createBuilderFunc,
                builder => builder.AddSingleton<ITestService, TestServiceWithNoCtorParam>(),
                CheckSingletonService<ITestService, TestServiceWithNoCtorParam>);
            TestRegistration(createBuilderFunc,
                builder => builder.AddSingleton<TestServiceWithNoCtorParam>(), CheckSingletonService<TestServiceWithNoCtorParam, TestServiceWithNoCtorParam>);
            TestRegistration(createBuilderFunc,
                builder => builder.AddSingleton(typeof(TestServiceWithNoCtorParam)), CheckSingletonService<TestServiceWithNoCtorParam, TestServiceWithNoCtorParam>);
            TestRegistration(createBuilderFunc,
                builder => builder.AddSingleton(typeof(ITestService), typeof(TestServiceWithNoCtorParam)),
                CheckSingletonService<ITestService, TestServiceWithNoCtorParam>);
            TestRegistration(createBuilderFunc,
                builder => builder.AddSingleton<ITestService>(c => new TestServiceWithNoCtorParam()),
                CheckSingletonService<ITestService, TestServiceWithNoCtorParam>);
        }

        public void TestAddScoped(Func<IDiContainerBuilder> createBuilderFunc)
        {
            TestScopedRegistration(createBuilderFunc,
                builder => builder.AddScoped<ITestService, TestServiceWithNoCtorParam>(),
                CheckScopedService<ITestService, TestServiceWithNoCtorParam>);
            TestScopedRegistration(createBuilderFunc,
                builder => builder.AddScoped<TestServiceWithNoCtorParam>(),
                CheckScopedService<TestServiceWithNoCtorParam, TestServiceWithNoCtorParam>);
            TestScopedRegistration(createBuilderFunc,
                builder => builder.AddScoped(typeof(TestServiceWithNoCtorParam)),
                CheckScopedService<TestServiceWithNoCtorParam, TestServiceWithNoCtorParam>);
            TestScopedRegistration(createBuilderFunc,
                builder => builder.AddScoped(typeof(ITestService), typeof(TestServiceWithNoCtorParam)),
                CheckScopedService<ITestService, TestServiceWithNoCtorParam>);
            TestScopedRegistration(createBuilderFunc,
                builder => builder.AddScoped(c => CreateService<ITestService>()),
                CheckScopedService<ITestService, TestServiceWithNoCtorParam>);
        }

        private static TService CreateService<TService>()
            where TService : class
        {
            return new TestServiceWithNoCtorParam() as TService;
        }

        private void TestScopedRegistration(Func<IDiContainerBuilder> createBuilderFunc, Action<IDiContainerBuilder> addRegistration, Action<IDiContainer> checkService)
        {
            var builder = createBuilderFunc();

            addRegistration(builder);

            var container = builder.Build();

            checkService(container);

            var scopedContainer = container.CreateScope().Container;

            checkService(scopedContainer);
        }

        private void TestRegistration(Func<IDiContainerBuilder> createBuilderFunc, Action<IDiContainerBuilder> addRegistration, Action<IDiContainer> checkService)
        {
            var builder = createBuilderFunc();

            addRegistration(builder);

            var container = builder.Build();

            checkService(container);
        }

        private void CheckTransientService<TService, TImplementation>(IDiContainer container)
            where TService : class
            where TImplementation : class
        {
            var service = container.GetInstance<TService>();

            Xunit.Assert.IsType<TImplementation>(service);

            var service2 = container.GetInstance<TService>();

            Xunit.Assert.NotEqual(service, service2);
        }

        private void CheckSingletonService<TService, TImplementation>(IDiContainer container)
            where TService : class
            where TImplementation : class
        {
            var service = container.GetInstance<TService>();

            Xunit.Assert.IsType<TImplementation>(service);

            var service2 = container.GetInstance<TService>();

            Xunit.Assert.Equal(service, service2);
        }

        private void CheckScopedService<TService, TImplementation>(IDiContainer container)
            where TService : class
            where TImplementation : class
        {
            var service = container.GetInstance<TService>();

            Xunit.Assert.IsType<TImplementation>(service);

            var service2 = container.GetInstance<TService>();

            Xunit.Assert.Equal(service, service2);
        }

        public void TestAddTransientNamed(Func<IDiContainerBuilder> createBuilderFunc)
        {
            var builder = createBuilderFunc();

            builder.AddTransientNamed<ITestService>()
                .Add<TestService2>("2")
                .Add<TestServiceWithNoCtorParam>("WithNoCtorParam")
                .Build();

            var container = builder.Build();

            Xunit.Assert.Throws<ResolveFailedException>(() => container.GetInstance<ITestService>("NotKnown"));

            Xunit.Assert.Throws<ResolveFailedException>(() => container.GetInstance(typeof(ITestService), "NotKnown"));

            Xunit.Assert.Throws<ResolveFailedException>(() => container.GetInstance<IUnknownService>("NotKnown"));

            Xunit.Assert.Throws<ResolveFailedException>(() => container.GetInstance(typeof(IUnknownService), "NotKnown"));

            var service2 = container.GetInstance<ITestService>("2");

            Xunit.Assert.IsType<TestService2>(service2);

            var serviceWithNoCtorParam = container.GetInstance<ITestService>("WithNoCtorParam");

            var serviceWithNoCtorParam2 = container.GetInstance(typeof(ITestService), "WithNoCtorParam");

            Xunit.Assert.IsType<TestServiceWithNoCtorParam>(serviceWithNoCtorParam);

            Xunit.Assert.IsType<TestServiceWithNoCtorParam>(serviceWithNoCtorParam2);

            var services = container.GetInstances<ITestService>().ToArray();

            Xunit.Assert.Empty(services);

            var serviceObjects = container.GetInstances(typeof(ITestService)).ToArray();

            Xunit.Assert.Empty(serviceObjects);
        }

        public void TestAddScopedNamed(Func<IDiContainerBuilder> createBuilderFunc)
        {
            var builder = createBuilderFunc();

            builder.AddScopedNamed<ITestService>()
                .Add<TestService2>("2")
                .Add<TestServiceWithNoCtorParam>("WithNoCtorParam")
                .Build();

            var container = builder.Build();

            Xunit.Assert.Throws<ResolveFailedException>(() => container.GetInstance<ITestService>("NotKnown"));

            Xunit.Assert.Throws<ResolveFailedException>(() => container.GetInstance(typeof(ITestService), "NotKnown"));

            Xunit.Assert.Throws<ResolveFailedException>(() => container.GetInstance<IUnknownService>("NotKnown"));

            Xunit.Assert.Throws<ResolveFailedException>(() => container.GetInstance(typeof(IUnknownService), "NotKnown"));

            var service2 = container.GetInstance<ITestService>("2");

            Xunit.Assert.IsType<TestService2>(service2);

            var serviceWithNoCtorParam = container.GetInstance<ITestService>("WithNoCtorParam");

            var serviceWithNoCtorParam2 = container.GetInstance(typeof(ITestService), "WithNoCtorParam");

            Xunit.Assert.IsType<TestServiceWithNoCtorParam>(serviceWithNoCtorParam);

            Xunit.Assert.IsType<TestServiceWithNoCtorParam>(serviceWithNoCtorParam2);

            var service21 = container.GetInstance<ITestService>("2");

            Xunit.Assert.IsType<TestService2>(service2);

            var serviceWithNoCtorParam1 = container.GetInstance<ITestService>("WithNoCtorParam");

            Xunit.Assert.IsType<TestServiceWithNoCtorParam>(serviceWithNoCtorParam);

            Xunit.Assert.Equal(service21, service2);

            Xunit.Assert.Equal(serviceWithNoCtorParam, serviceWithNoCtorParam1);
        }

        public void TestAddSingletonNamed(Func<IDiContainerBuilder> createBuilderFunc)
        {
            var builder = createBuilderFunc();

            builder.AddSingletonNamed<ITestService>()
                .Add<TestService2>("2")
                .Add<TestServiceWithNoCtorParam>("WithNoCtorParam")
                .Build();

            var container = builder.Build();

            Xunit.Assert.Throws<ResolveFailedException>(() => container.GetInstance<ITestService>("NotKnown"));

            Xunit.Assert.Throws<ResolveFailedException>(() => container.GetInstance(typeof(ITestService), "NotKnown"));

            Xunit.Assert.Throws<ResolveFailedException>(() => container.GetInstance<IUnknownService>("NotKnown"));

            Xunit.Assert.Throws<ResolveFailedException>(() => container.GetInstance(typeof(IUnknownService), "NotKnown"));

            var service2 = container.GetInstance<ITestService>("2");

            Xunit.Assert.IsType<TestService2>(service2);

            var serviceWithNoCtorParam = container.GetInstance<ITestService>("WithNoCtorParam");

            var serviceWithNoCtorParam2 = container.GetInstance(typeof(ITestService), "WithNoCtorParam");

            Xunit.Assert.IsType<TestServiceWithNoCtorParam>(serviceWithNoCtorParam);

            Xunit.Assert.IsType<TestServiceWithNoCtorParam>(serviceWithNoCtorParam2);

            var service21 = container.GetInstance<ITestService>("2");

            Xunit.Assert.IsType<TestService2>(service2);

            var serviceWithNoCtorParam1 = container.GetInstance<ITestService>("WithNoCtorParam");

            Xunit.Assert.IsType<TestServiceWithNoCtorParam>(serviceWithNoCtorParam);

            Xunit.Assert.Equal(service21, service2);

            Xunit.Assert.Equal(serviceWithNoCtorParam, serviceWithNoCtorParam1);
        }

        public void TestAddTransientNamedTestNoRegistration(Func<IDiContainerBuilder> createBuilderFunc)
        {
            var builder = createBuilderFunc();

            builder.AddTransientNamed<ITestService>().Build();

            var container = builder.Build();

            Xunit.Assert.Throws<ResolveFailedException>(() => container.GetInstance<ITestService>("2"));

            Xunit.Assert.Throws<ResolveFailedException>(() => container.GetInstance(typeof(ITestService), "2"));
        }

        public void GetInstance_GetDiContainerInScope_GetsScopedContainer(Func<IDiContainerBuilder> createBuilderFunc)
        {
            var builder = createBuilderFunc();
            builder.AddScoped<ITestService, TestService2>();

            var container = builder.Build();

            var scopedContainer = container.CreateScope().Container;

            var scopedTestService = TestGetInstanceWithScopedServices(scopedContainer);

            var secondScopedContainer = container.CreateScope().Container;

            var secondScopedTestService = TestGetInstanceWithScopedServices(secondScopedContainer);

            Xunit.Assert.NotSame(scopedTestService, secondScopedTestService);
        }

        private static ITestService TestGetInstanceWithScopedServices(IDiContainer diContainer)
        {
            var scopedTestService = diContainer.GetInstance<ITestService>();

            var scopedTestService2 = diContainer.GetInstance<ITestService>();

            var scopedContainerFromContainer = diContainer.GetInstance<IDiContainer>();

            var scopedTestServiceFromScopedDiContainer = scopedContainerFromContainer.GetInstance<ITestService>();

            Xunit.Assert.Same(scopedTestService, scopedTestServiceFromScopedDiContainer);

            Xunit.Assert.Same(scopedTestService, scopedTestService2);

            return scopedTestService;
        }

        public void GetInstances_CollectionAndNamedRegistrations_ReturnsAllInstances(Func<IDiContainerBuilder> createBuilderFunc)
        {
            var builder = createBuilderFunc();

            builder
                .AddTransientCollection<ITestService>(typeof(TestService), typeof(TestService2))
                .AddTransientNamed<ITestService>().Add<TestServiceWithNoCtorParam>("Tester").Build();

            var container = builder.Build();

            var services = container.GetInstances<ITestService>().ToArray();
            var serviceContainer = container.GetInstance<TestServiceContainer>();

            Xunit.Assert.Equal(2, services.Length);
            Xunit.Assert.Equal(2, serviceContainer.Services.Count());

            Xunit.Assert.Contains(services, service => service is TestService);
            Xunit.Assert.Contains(services, service => service is TestService2);

            var serviceObjects = container.GetInstances(typeof(ITestService)).ToArray();

            Xunit.Assert.Equal(2, serviceObjects.Length);

            Xunit.Assert.Contains(serviceObjects, service => service is TestService);
            Xunit.Assert.Contains(serviceObjects, service => service is TestService2);            
        }

        public void ClassFactory_GetFactoryForInterface_ReturnsInstance(Func<IDiContainerBuilder> createBuilderFunc)
        {
            var containerBuilder = createBuilderFunc();
            containerBuilder.AddTransient<ITestService, TestService>();

            var container = containerBuilder.Build();

            var serviceFactory = container.GetInstance<IClassFactory<ITestService>>();

            var testService = serviceFactory.Create();

            Xunit.Assert.NotNull(testService);
            Xunit.Assert.IsType<TestService>(testService);

            var testServiceWithProperty = serviceFactory.Create(x => x.Text = "Hello World");

            Xunit.Assert.Equal("Hello World", testServiceWithProperty.Text);

            var serviceFactory2 = container.GetInstance<IClassFactory<TestService2>>();

            var testService2 = serviceFactory2.Create();            

            Xunit.Assert.NotNull(testService2);
            Xunit.Assert.IsType<TestService2>(testService2);

            var testServiceWithProperty2 = serviceFactory2.Create(x => x.Text = "This is a test");

            Xunit.Assert.Equal("This is a test", testServiceWithProperty2.Text);
        }

        public void AddSingleton_OpenGeneric_GetInstanceReturnsSameInstance(Func<IDiContainerBuilder> createBuilderFunc)
        {
            var builder = createBuilderFunc();

            builder.AddSingleton(typeof(ITestService<>), typeof(TestService<>));

            var container = builder.Build();

            var instanceInt = container.GetInstance<ITestService<int>>();

            var instanceInt2 = container.GetInstance<ITestService<int>>();

            var instanceString = container.GetInstance<ITestService<string>>();

            Xunit.Assert.Same(instanceInt, instanceInt2);

            Xunit.Assert.NotSame(instanceInt, instanceString);
        }

        public void AddTransient_OpenGeneric_GetInstanceReturnsDifferentInstances(Func<IDiContainerBuilder> createBuilderFunc)
        {
            var builder = createBuilderFunc();

            builder.AddTransient(typeof(ITestService<>), typeof(TestService<>));

            var container = builder.Build();

            var instanceInt = container.GetInstance<ITestService<int>>();

            var instanceInt2 = container.GetInstance<ITestService<int>>();

            var instanceString = container.GetInstance<ITestService<string>>();

            Xunit.Assert.NotSame(instanceInt, instanceInt2);

            Xunit.Assert.NotSame(instanceInt, instanceString);

            Xunit.Assert.NotSame(instanceInt2, instanceString);
        }

        public void RegisterImplementations_ForAllAssemblies_ReturnsCorrectInstances(Func<IDiContainerBuilder> createBuilderFunc)
        {
            var builder = createBuilderFunc();

            builder.RegisterImplementations();

            var container = builder.Build();

            var instance = container.GetInstance<ITestService>();

            var instance2 = container.GetInstance<ITestService>();

            Xunit.Assert.IsType<TestService>(instance);

            Xunit.Assert.NotSame(instance, instance2);

            var singletonInstance = container.GetInstance<ITestService<int>>();

            var singletonInstance2 = container.GetInstance<ITestService<int>>();

            Xunit.Assert.IsType<TestService<int>>(singletonInstance);

            Xunit.Assert.Same(singletonInstance, singletonInstance2);
        }

        public void RegisterImplementations_ForAssembly_ReturnsCorrectInstance(Func<IDiContainerBuilder> createBuilderFunc)
        {
            var builder = createBuilderFunc();

            builder.RegisterImplementations(typeof(ITestService).Assembly);

            var container = builder.Build();

            var instance = container.GetInstance<ITestService>();

            var instance2 = container.GetInstance<ITestService>();

            Xunit.Assert.IsType<TestService>(instance);

            Xunit.Assert.NotSame(instance, instance2);

            var singletonInstance = container.GetInstance<ITestService<int>>();

            var singletonInstance2 = container.GetInstance<ITestService<int>>();

            Xunit.Assert.IsType<TestService<int>>(singletonInstance);

            Xunit.Assert.Same(singletonInstance, singletonInstance2);

            var scopedInstance = container.GetInstance<IUnknownService>();

            var scopedInstance2 = container.GetInstance<IUnknownService>();

            Xunit.Assert.IsType<UnknownService>(scopedInstance);

            Xunit.Assert.Equal(scopedInstance, scopedInstance2);
        }

        public void RegisterImplementations_ForAssemblies_ReturnsCorrectInstances(Func<IDiContainerBuilder> createBuilderFunc)
        {
            var builder = createBuilderFunc();

            builder.RegisterImplementations(new []{typeof(EnumerableExtension).Assembly, typeof(ITestService).Assembly});

            var container = builder.Build();

            var instance = container.GetInstance<ITestService>();

            var instance2 = container.GetInstance<ITestService>();

            Xunit.Assert.IsType<TestService>(instance);

            Xunit.Assert.NotSame(instance, instance2);

            var singletonInstance = container.GetInstance<ITestService<int>>();

            var singletonInstance2 = container.GetInstance<ITestService<int>>();

            Xunit.Assert.IsType<TestService<int>>(singletonInstance);

            Xunit.Assert.Same(singletonInstance, singletonInstance2);
        }

        public void RegisterImplementations_ForType_ReturnsCorrectInstance(Func<IDiContainerBuilder> createBuilderFunc)
        {
            var builder = createBuilderFunc();

            builder.RegisterImplementations(new []{typeof(TestService)});

            var container = builder.Build();

            var instance = container.GetInstance<ITestService>();

            var instance2 = container.GetInstance<ITestService>();

            Xunit.Assert.IsType<TestService>(instance);

            Xunit.Assert.NotSame(instance, instance2);
        }
    }
}