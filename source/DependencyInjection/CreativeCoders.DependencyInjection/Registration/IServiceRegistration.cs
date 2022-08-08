using Microsoft.Extensions.DependencyInjection;

namespace CreativeCoders.DependencyInjection.Registration;

public interface IServiceRegistration
{
    void ConfigureServices(IServiceCollection services);
}
