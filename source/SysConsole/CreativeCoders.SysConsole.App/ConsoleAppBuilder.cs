using System;
using CreativeCoders.SysConsole.Core.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CreativeCoders.SysConsole.App
{
    public class ConsoleAppBuilder
    {
        private readonly string[] _arguments;

        private Type? _startupType;

        private Action<ConfigurationBuilder>? _setupConfiguration;

        private Action<IServiceCollection>? _configureServices;

        private Func<IServiceProvider, ICommandExecutor>? _createExecutor;

        public ConsoleAppBuilder(string[]? arguments)
        {
            _arguments = arguments ?? Array.Empty<string>();
        }

        public ConsoleAppBuilder UseStartup<TStartup>()
            where TStartup : IStartup, new()
        {
            _startupType = typeof(TStartup);

            return this;
        }

        public ConsoleAppBuilder UseConfiguration(
            Action<ConfigurationBuilder> setupConfiguration)
        {
            _setupConfiguration = setupConfiguration;
            
            return this;
        }

        public ConsoleAppBuilder ConfigureServices(Action<IServiceCollection> configureServices)
        {
            _configureServices = configureServices;

            return this;
        }

        public ConsoleAppBuilder UseExecutor(Func<IServiceProvider, ICommandExecutor> createExecutor)
        {
            if (_createExecutor != null)
            {
                throw new InvalidOperationException("Executor already set");
            }

            _createExecutor = createExecutor;

            return this;
        }

        public IConsoleApp Build()
        {
            if (_createExecutor == null)
            {
                throw new InvalidOperationException("No executor defined");
            }

            var serviceProvider = CreateServiceProvider();

            var commandExecutor = _createExecutor(serviceProvider);

            return new DefaultConsoleApp(commandExecutor, _arguments, serviceProvider.GetRequiredService<ISysConsole>());
        }

        private void ConfigureStartup(IServiceCollection services, IConfiguration configuration)
        {
            if (_startupType == null)
            {
                return;
            }

            if (Activator.CreateInstance(_startupType) is not IStartup startup)
            {
                throw new ArgumentException("Startup could not be created", nameof(_startupType));
            }

            startup.ConfigureServices(services, configuration);

            services.AddSingleton(startup);
        }

        private IServiceProvider CreateServiceProvider()
        {
            var services = new ServiceCollection();

            services.AddSysConsole();

            var configuration = AddConfiguration(services);

            _configureServices?.Invoke(services);

            ConfigureStartup(services, configuration);

            return services.BuildServiceProvider();
        }

        private IConfigurationRoot AddConfiguration(IServiceCollection services)
        {
            var configurationBuilder = new ConfigurationBuilder();

            _setupConfiguration?.Invoke(configurationBuilder);

            var configuration = configurationBuilder.Build();

            services.AddSingleton(configuration);
            services.AddSingleton<IConfiguration>(configuration);

            return configuration;
        }
    }
}
