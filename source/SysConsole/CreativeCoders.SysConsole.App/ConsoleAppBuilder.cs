using System;
using CreativeCoders.Core;
using CreativeCoders.SysConsole.App.Execution;
using CreativeCoders.SysConsole.App.MainProgram;
using CreativeCoders.SysConsole.App.VerbObjects;
using CreativeCoders.SysConsole.App.Verbs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CreativeCoders.SysConsole.App
{
    public class ConsoleAppBuilder
    {
        private readonly string[] _arguments;

        private Type? _startupType;

        private Type? _programMainType;
        
        private Action<IConsoleAppVerbsBuilder>? _setupVerbsBuilder;

        private Action<ConfigurationBuilder>? _setupConfiguration;

        private Action<IConsoleAppVerbObjectsBuilder>? _setupVerbObjectsBuilder;

        private Action<IServiceCollection>? _configureServices;

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

        public ConsoleAppBuilder UseVerbs(
            Action<IConsoleAppVerbsBuilder> verbBuilder)
        {
            _setupVerbsBuilder = Ensure.Argument(verbBuilder, nameof(verbBuilder)).NotNull();

            return this;
        }

        public ConsoleAppBuilder UseVerbObjects(Action<IConsoleAppVerbObjectsBuilder> verbObjectsBuilder)
        {
            _setupVerbObjectsBuilder = verbObjectsBuilder;

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
            _programMainType = typeof(TProgramMain);

            return this;
        }

        public ConsoleAppBuilder Configure(Action<IServiceCollection> configureServices)
        {
            _configureServices = configureServices;

            return this;
        }

        public IConsoleApp Build()
        {
            if (_setupVerbsBuilder == null && _setupVerbObjectsBuilder == null && _programMainType == null)
            {
                throw new ArgumentException("No program main or verb defined");
            }

            var serviceProvider = CreateServiceProvider();

            var executorChain = new ExecutorChain(_programMainType, _setupVerbsBuilder,
                _setupVerbObjectsBuilder, serviceProvider);

            var commandExecutor = new CommandExecutor(executorChain);

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
