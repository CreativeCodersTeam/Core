using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CreativeCoders.SysConsole.App;

/// <summary>   Interface for console app startup. </summary>
public interface IStartup
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Add services to dependency injection container. </summary>
    ///
    /// <param name="services">         The services. </param>
    /// <param name="configuration">    The configuration. </param>
    ///-------------------------------------------------------------------------------------------------
    void ConfigureServices(IServiceCollection services, IConfiguration configuration);
}