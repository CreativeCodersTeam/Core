using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CreativeCoders.SysConsole.App
{
    public interface IStartup
    {
        void ConfigureServices(IServiceCollection services, IConfiguration configuration);
    }
}