using System;
using CreativeCoders.Core.Reflection;
using CreativeCoders.SysConsole.App.Execution;
using CreativeCoders.SysConsole.App.MainProgram;
using CreativeCoders.SysConsole.CliArguments.Building;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CreativeCoders.SysConsole.App
{
    public class ConsoleAppBuilder
    {
        private readonly string[] _arguments;

        private Type? _startupType;

        private Type? _programMainType;
        
        private Action<ConfigurationBuilder>? _setupConfiguration;

        private Action<IServiceCollection>? _configureServices;

        private Action<ICliBuilder>? _setupCliBuilder;

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

        public ConsoleAppBuilder UseProgramMain<TProgramMain>()
            where TProgramMain : IMain
        {
            if (_setupCliBuilder != null)
            {
                throw new InvalidOperationException("CLI builder is already configured");
            }

            _programMainType = typeof(TProgramMain);

            return this;
        }

        public ConsoleAppBuilder Configure(Action<IServiceCollection> configureServices)
        {
            _configureServices = configureServices;

            return this;
        }

        public ConsoleAppBuilder UseCli(Action<ICliBuilder> setupCliBuilder)
        {
            if (_programMainType != null)
            {
                throw new InvalidOperationException("Program main is already set");
            }

            _setupCliBuilder = setupCliBuilder;

            return this;
        }

        public IConsoleApp Build()
        {
            if (_programMainType == null && _setupCliBuilder == null)
            {
                throw new InvalidOperationException("No program main and no CLI builder defined");
            }

            var serviceProvider = CreateServiceProvider();

            ICommandExecutor commandExecutor = _programMainType != null
                ? new MainExecutor(_programMainType!.CreateInstance<IMain>(serviceProvider))
                : new CliBuilderExecutor(_setupCliBuilder!, serviceProvider);

            return new DefaultConsoleApp(commandExecutor, _arguments);
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
