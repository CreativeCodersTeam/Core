using System;
using CreativeCoders.Core;
using CreativeCoders.SysConsole.App.Verbs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CreativeCoders.SysConsole.App
{
    public class ConsoleApplicationBuilder
    {
        private readonly string[] _arguments;

        private Type? _startupType;

        private Type? _programMainType;
        
        private Action<IConsoleApplicationVerbBuilder>? _setupVerbBuilder;

        private Action<ConfigurationBuilder>? _setupConfiguration;

        public ConsoleApplicationBuilder(string[]? arguments)
        {
            _arguments = arguments ?? Array.Empty<string>();
        }

        public ConsoleApplicationBuilder UseStartup<TStartup>()
            where TStartup : IStartup, new()
        {
            _startupType = typeof(TStartup);

            return this;
        }

        public ConsoleApplicationBuilder UseVerbs(
            Action<IConsoleApplicationVerbBuilder> verbBuilder)
        {
            _setupVerbBuilder = Ensure.Argument(verbBuilder, nameof(verbBuilder)).NotNull();

            return this;
        }

        public ConsoleApplicationBuilder UseConfiguration(
            Action<ConfigurationBuilder> setupConfiguration)
        {
            _setupConfiguration = setupConfiguration;
            
            return this;
        }

        public ConsoleApplicationBuilder UseProgramMain<TProgramMain>()
        {
            _programMainType = typeof(TProgramMain);

            return this;
        }

        public IConsoleApplication Build()
        {
            if (_setupVerbBuilder == null && _programMainType == null)
            {
                throw new ArgumentException("No program main or verb defined");
            }

            var serviceProvider = GetServiceProvider();

            var main = CreateMain(serviceProvider);

            return new DefaultConsoleApplication(main);
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

            if (_setupVerbBuilder == null)
            {
                if (main == null)
                {
                    throw new ArgumentException("Program main could not be created");
                }

                return main;
            }
            
            var verbBuilder = new DefaultConsoleApplicationVerbBuilder();

            _setupVerbBuilder(verbBuilder);

            return verbBuilder.BuildMain(main, _arguments, serviceProvider);
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