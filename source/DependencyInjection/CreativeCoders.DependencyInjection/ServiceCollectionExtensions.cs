using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using CreativeCoders.Core.Collections;
using CreativeCoders.Core.Reflection;
using CreativeCoders.DependencyInjection.Registration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CreativeCoders.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void ConfigureServicesFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        var registrations = typeof(IServiceRegistration)
            .GetImplementations(assembly)
            .Select(CreateServiceRegistration);

        registrations.ForEach(x => x.ConfigureServices(services));
    }

    [ExcludeFromCodeCoverage]
    private static IServiceRegistration CreateServiceRegistration(Type serviceRegistrationType)
    {
        if (Activator.CreateInstance(serviceRegistrationType) is not IServiceRegistration serviceRegistration)
        {
            throw new InvalidOperationException();
        }

        return serviceRegistration;
    }

    public static void AddObjectFactory(this IServiceCollection services)
    {
        services.TryAddSingleton<IObjectFactory, DefaultObjectFactory>();

        services.TryAddSingleton(typeof(IObjectFactory<>), typeof(DefaultObjectFactory<>));
    }
}
