using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CreativeCoders.SysConsole.App.UnitTests.TestData;

public class TestStartup : IStartup
{
    public void ConfigureServices(IServiceCollection? services, IConfiguration? configuration)
    {
        ConfigureServicesIsCalled = services != null && configuration != null;
    }

    public static bool ConfigureServicesIsCalled { get; private set; }
}