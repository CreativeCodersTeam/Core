using CreativeCoders.Daemon.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CreativeCoders.Daemon.Windows;

///-------------------------------------------------------------------------------------------------
/// <summary>   The windows daemon host builder. </summary>
///
/// <typeparam name="TDaemonService">   Type of the daemon service. </typeparam>
///
/// <seealso cref="DaemonHostBuilder{TDaemonService}"/>
///-------------------------------------------------------------------------------------------------
public class WindowsDaemonHostBuilder<TDaemonService> : DaemonHostBuilder<TDaemonService>
    where TDaemonService : class, IDaemonService
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Is called to add services to container. Can be overridden in inherited classes. Adds the
    ///     event log logger.
    /// </summary>
    ///
    /// <param name="services"> The services. </param>
    ///
    /// <seealso cref="DaemonHostBuilder{TDaemonService}.ConfigureServices(IServiceCollection)"/>
    /// <seealso cref="WorkerHostBuilder{DaemonWorker}.ConfigureServices(IServiceCollection)"/>
    ///-------------------------------------------------------------------------------------------------
    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);

        services.AddLogging(x => x.AddEventLog());
    }
}

///-------------------------------------------------------------------------------------------------
/// <summary>   The windows daemon host builder. </summary>
///
/// <typeparam name="TDaemonService">   Type of the daemon service. </typeparam>
/// <typeparam name="TConfig">          Type of the configuration. </typeparam>
///
/// <seealso cref="DaemonHostBuilder{TDaemonService,TConfig}"/>
///-------------------------------------------------------------------------------------------------
public class WindowsDaemonHostBuilder<TDaemonService, TConfig> : DaemonHostBuilder<TDaemonService, TConfig>
    where TDaemonService : class, IDaemonService
    where TConfig : class
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Initializes a new instance of the <see cref="WindowsDaemonHostBuilder{TDaemonService,TConfig}"/>
    ///     CreativeCoders.Daemon.Windows.WindowsDaemonHostBuilder&lt;TDaemonService, TConfig&gt;
    ///     class.
    /// </summary>
    ///
    /// <param name="workerConfig"> The worker configuration. </param>
    ///-------------------------------------------------------------------------------------------------
    public WindowsDaemonHostBuilder(TConfig workerConfig) : base(workerConfig) { }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Is called to add services to container. Can be overridden in inherited classes. Adds the
    ///     worker config to the container.
    /// </summary>
    ///
    /// <param name="services"> The services. </param>
    ///
    /// <seealso cref="DaemonHostBuilder{TDaemonService,TConfig}.ConfigureServices(IServiceCollection)"/>
    /// <seealso cref="WorkerHostBuilder{TDaemonService,TConfig}.ConfigureServices(IServiceCollection)"/>
    ///-------------------------------------------------------------------------------------------------
    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);

        services.AddLogging(x => x.AddEventLog());
    }
}