using CreativeCoders.DependencyInjection.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace CreativeCoders.DependencyInjection.UnitTests.TestData;

public class TestServiceRegistration : IServiceRegistration
{
    public void ConfigureServices(IServiceCollection services)
    {
        Services = services;
    }

    public static IServiceCollection? Services { get; private set; }
}
