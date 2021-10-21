using System;
using CreativeCoders.Core;
using CreativeCoders.Core.Reflection;
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
        
        private Action<IConsoleAppVerbBuilder>? _setupVerbBuilder;

        private Action<ConfigurationBuilder>? _setupConfiguration;

        private Action<IConsoleAppVerbObjectsBuilder>? _setupVerbObjectsBuilder;

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
            Action<IConsoleAppVerbBuilder> verbBuilder)
        {
            _setupVerbBuilder = Ensure.Argument(verbBuilder, nameof(verbBuilder)).NotNull();

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
        {
            _programMainType = typeof(TProgramMain);

            return this;
        }

        public IConsoleApp Build()
        {
            if (_setupVerbBuilder == null && _setupVerbObjectsBuilder == null && _programMainType == null)
            {
                throw new ArgumentException("No program main or verb defined");
            }

            var serviceProvider = GetServiceProvider();

            var main = CreateCombinedMain(CreateMain(serviceProvider), serviceProvider);

            return new DefaultConsoleApp(main);
        }

        private IServiceProvider GetServiceProvider()
        {
            if (_startupType == null)
            {
                return CreateServiceProvider(null);
            }

            if (Activator.CreateInstance(_startupType) is not IStartup startup)
            {
                throw new ArgumentException("Startup could not be created", nameof(_startupType));
            }

            return CreateServiceProvider(startup);
        }

        private IMain CreateMain(IServiceProvider serviceProvider)
        {
            var main = _programMainType?.CreateInstance<IMain>(serviceProvider);

            if (_setupVerbBuilder == null && _setupVerbObjectsBuilder == null)
            {
                if (main == null)
                {
                    throw new ArgumentException("Program main could not be created");
                }

                return main;
            }

            if (_setupVerbBuilder != null)
            {
                var verbBuilder = new DefaultConsoleAppVerbBuilder();

                _setupVerbBuilder(verbBuilder);

                main = verbBuilder.BuildMain(main, _arguments, serviceProvider);
            }

            main = CreateCombinedMain(main, serviceProvider);

            return main;
        }

        private IMain CreateCombinedMain(IMain? defaultMain, IServiceProvider serviceProvider)
        {
            if (_setupVerbObjectsBuilder == null)
            {
                if (defaultMain == null)
                {
                    throw new ArgumentException("Program main could not be created");
                }

                return defaultMain;
            }

            var verbObjectsBuilder = new DefaultConsoleAppVerbObjectsBuilder();

            _setupVerbObjectsBuilder(verbObjectsBuilder);

            return new DelegateMain(async () =>
            {
                var executionResult = await verbObjectsBuilder.TryExecute(serviceProvider, _arguments);

                if (executionResult.HasBeenExecuted)
                {
                    return executionResult.ReturnCode;
                }

                return defaultMain != null
                    ? await defaultMain.ExecuteAsync()
                    : int.MinValue;
            });
        }

        private IServiceProvider CreateServiceProvider(IStartup? startup)
        {
            var services = new ServiceCollection();

            services.AddSysConsole();

            var configuration = AddConfiguration(services);

            startup?.ConfigureServices(services, configuration);

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
