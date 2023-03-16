using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CreativeCoders.Daemon;

public interface IDaemonHostBuilder
{
    IDaemonHostBuilder WithArgs(string[] args);

    IDaemonHostBuilder ConfigureServices(Action<IServiceCollection> configureServices);

    IDaemonHostBuilder ConfigureHostBuilder(Action<IHostBuilder> configureHostBuilder);

    IDaemonHostBuilder WithInstaller<TInstaller>()
        where TInstaller : class, IDaemonInstaller;

    IDaemonHostBuilder WithDefinitionFile(string fileName);

    IDaemonHost Build();
}
