using System;
using CreativeCoders.SysConsole.Core.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CreativeCoders.SysConsole.App;

/// <summary>   A console application builder. </summary>
public class ConsoleAppBuilder
{
    private readonly string[] _arguments;

    private Type? _startupType;

    private Action<ConfigurationBuilder>? _setupConfiguration;

    private Action<IServiceCollection>? _configureServices;

    private Func<IServiceProvider, IConsoleAppExecutor>? _createExecutor;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Initializes a new instance of the CreativeCoders.SysConsole.App.ConsoleAppBuilder class.
    /// </summary>
    ///
    /// <param name="arguments">    The command line arguments. </param>
    ///-------------------------------------------------------------------------------------------------
    public ConsoleAppBuilder(string[]? arguments)
    {
        _arguments = arguments ?? Array.Empty<string>();
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Use startup class for adding services to dependency injection container. </summary>
    ///
    /// <typeparam name="TStartup"> Type of the startup class. </typeparam>
    ///
    /// <returns>   This ConsoleAppBuilder. </returns>
    ///-------------------------------------------------------------------------------------------------
    public ConsoleAppBuilder UseStartup<TStartup>()
        where TStartup : IStartup, new()
    {
        _startupType = typeof(TStartup);

        return this;
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Setup configuration system. </summary>
    ///
    /// <param name="setupConfiguration">   Action for setting up the configuration system. </param>
    ///
    /// <returns>   This ConsoleAppBuilder. </returns>
    ///-------------------------------------------------------------------------------------------------
    public ConsoleAppBuilder UseConfiguration(
        Action<ConfigurationBuilder> setupConfiguration)
    {
        _setupConfiguration = setupConfiguration;
            
        return this;
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Configure services. </summary>
    ///
    /// <param name="configureServices">    Action for configuring services. </param>
    ///
    /// <returns>   This ConsoleAppBuilder. </returns>
    ///-------------------------------------------------------------------------------------------------
    public ConsoleAppBuilder ConfigureServices(Action<IServiceCollection> configureServices)
    {
        _configureServices = configureServices;

        return this;
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Specify the function for creating the executor which should be used for the console app.
    /// </summary>
    ///
    /// <exception cref="InvalidOperationException">    Thrown when an executor is already registered. </exception>
    ///
    /// <param name="createExecutor">   The function for creating the executor. </param>
    ///
    /// <returns>   This ConsoleAppBuilder. </returns>
    ///-------------------------------------------------------------------------------------------------
    public ConsoleAppBuilder UseExecutor(Func<IServiceProvider, IConsoleAppExecutor> createExecutor)
    {
        if (_createExecutor != null)
        {
            throw new InvalidOperationException("Executor already set");
        }

        _createExecutor = createExecutor;

        return this;
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Builds the console app. </summary>
    ///
    /// <exception cref="InvalidOperationException">    Thrown when no executor is registered. </exception>
    ///
    /// <returns>   The console app <see cref="IConsoleApp"/>. </returns>
    ///-------------------------------------------------------------------------------------------------
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