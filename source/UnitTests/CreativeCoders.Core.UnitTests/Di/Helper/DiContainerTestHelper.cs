using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CreativeCoders.Di;
using CreativeCoders.Di.Exceptions;

namespace CreativeCoders.Core.UnitTests.Di.Helper;

[SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Global")]
public class DiContainerTestHelper
{
    public void TestGetInstance<TService, TImplementation>(IDiContainer container)
        where TService : class
        where TImplementation : class
    {
        Xunit.Assert.Throws<ResolveFailedException>(container.GetInstance<IUnknownService>);

        var helper = container.GetInstance<DiContainerTestHelper>();

        Xunit.Assert.IsType<DiContainerTestHelper>(helper);

        var service = container.GetInstance<TService>();

        Xunit.Assert.IsType<TImplementation>(service);

        var service2 = container.GetInstance(typeof(TService));

        Xunit.Assert.IsType<TImplementation>(service2);

        var service3 = container.GetService(typeof(TService));

        Xunit.Assert.IsType<TImplementation>(service3);
    }

    public void TestGetInstanceThrowsException<TService>(IDiContainer container)
        where TService : class
    {
        Xunit.Assert.Throws<ResolveFailedException>(container.GetInstance<TService>);

        Xunit.Assert.Throws<ResolveFailedException>(() => container.GetInstance(typeof(TService)));

        Xunit.Assert.Throws<ResolveFailedException>(() => container.GetInstance<TService>("1"));
    }

    public void TestGetInstances<TService>(IDiContainer container, params Type[] implementationTypes)
        where TService : class
    {
        var services = container.GetInstances<TService>().ToArray();

        Xunit.Assert.Equal(implementationTypes.Length, services.Length);

        CheckServices(services, implementationTypes);

        var serviceObjects = container.GetInstances(typeof(TService)).ToArray();

        Xunit.Assert.Equal(implementationTypes.Length, serviceObjects.Length);

        CheckServices(serviceObjects, implementationTypes);            
    }

    public void GetInstances_NoServicesFound_ReturnsEmptyArray<TService>(IDiContainer container)
        where TService : class
    {
        var services = container.GetInstances<TService>().ToArray();

        Xunit.Assert.Empty(services);

        var serviceObjects = container.GetInstances(typeof(TService));

        Xunit.Assert.Empty(serviceObjects);
    }

    public void TestCreateScope<TService, TImplementation>(IDiContainer container)
        where TService : class
    {
        using (var scope = container.CreateScope())
        {
            container = scope.Container;

            var service = container.GetInstance<TService>();

            Xunit.Assert.IsType<TImplementation>(service);

            var service2 = container.GetInstance<TService>();

            Xunit.Assert.IsType<TImplementation>(service2);

            Xunit.Assert.Equal(service, service2);

            using (var innerScope = container.CreateScope())
            {
                var scopedService = innerScope.Container.GetInstance<TService>();

                Xunit.Assert.IsType<TImplementation>(scopedService);

                var scopedService2 = innerScope.Container.GetInstance<TService>();

                Xunit.Assert.IsType<TImplementation>(scopedService2);

                Xunit.Assert.Equal(scopedService, scopedService2);

                Xunit.Assert.NotEqual(service, scopedService);
            }
        }
    }

    public void TestScopeDisposeDouble(IDiContainer container)
    {
        var scope = container.CreateScope();

        scope.Dispose();
        scope.Dispose();
    }

    public void TryGetInstance_NoServiceFound_ReturnsFalseAndNull<TService>(IDiContainer container)
        where TService : class
    {
        var serviceFound = container.TryGetInstance<TService>(out var instance);

        Xunit.Assert.False(serviceFound);
        Xunit.Assert.Null(instance);

        var serviceObjectFound = container.TryGetInstance(typeof(TService), out var objectInstance);

        Xunit.Assert.False(serviceObjectFound);
        Xunit.Assert.Null(objectInstance);
    }

    public void TryGetInstance_ServiceFound_ReturnsTrueAndInstance<TService>(IDiContainer container)
        where TService : class
    {
        var serviceFound = container.TryGetInstance<TService>(out var instance);

        Xunit.Assert.True(serviceFound);
        Xunit.Assert.NotNull(instance);

        var serviceObjectFound = container.TryGetInstance(typeof(TService), out var objectInstance);

        Xunit.Assert.True(serviceObjectFound);
        Xunit.Assert.NotNull(objectInstance);
    }

    // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
    private static void CheckServices<TService>(IEnumerable<TService> services, Type[] implementationTypes)
    {
        foreach (var service in services)
        {
            Xunit.Assert.Contains(service.GetType(), implementationTypes);
        }
    }
}