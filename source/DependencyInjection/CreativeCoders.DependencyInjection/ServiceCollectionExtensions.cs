using System.Reflection;
using CreativeCoders.Core.Collections;
using CreativeCoders.Core.Reflection;
using CreativeCoders.DependencyInjection.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace CreativeCoders.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void ConfigureServicesFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        var registrations = typeof(IServiceRegistration)
            .GetImplementations(new[] {assembly})
            .Select(x =>
            {
                if (Activator.CreateInstance(x) is not IServiceRegistration serviceRegistration)
                {
                    throw new InvalidOperationException();
                }

                return serviceRegistration;
            });

        registrations.ForEach(x => x.ConfigureServices(services));
    }
}
