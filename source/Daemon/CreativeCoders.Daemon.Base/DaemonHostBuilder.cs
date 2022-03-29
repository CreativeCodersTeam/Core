using Microsoft.Extensions.DependencyInjection;

namespace CreativeCoders.Daemon.Base;

///-------------------------------------------------------------------------------------------------
/// <summary>   A worker host builder for a specific daemon service implementation. </summary>
///
/// <typeparam name="TDaemonService">   Type of the daemon service. </typeparam>
///
/// <seealso cref="WorkerHostBuilder{DaemonWorker}"/>
///-------------------------------------------------------------------------------------------------
public class DaemonHostBuilder<TDaemonService> : WorkerHostBuilder<DaemonWorker>
    where TDaemonService : class, IDaemonService
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Is called to add services to container. Can be overridden in inherited classes. Adds the
    ///     daemon service to the container.
    /// </summary>
    ///
    /// <param name="services"> The services. </param>
    ///
    /// <seealso cref="WorkerHostBuilder{DaemonWorker}.ConfigureServices(IServiceCollection)"/>
    ///-------------------------------------------------------------------------------------------------
    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
        services.AddSingleton<IDaemonService, TDaemonService>();
    }
}

///-------------------------------------------------------------------------------------------------
/// <summary>   A worker host builder for a specific daemon service implementation and . </summary>
///
/// <typeparam name="TDaemonService">   Type of the daemon service. </typeparam>
/// <typeparam name="TConfig">          Type of the configuration. </typeparam>
///
/// <seealso cref="WorkerHostBuilder{DaemonWorker,TConfig}"/>
///-------------------------------------------------------------------------------------------------
public class DaemonHostBuilder<TDaemonService, TConfig> : WorkerHostBuilder<DaemonWorker, TConfig>
    where TDaemonService : class, IDaemonService
    where TConfig : class
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Initializes a new instance of the <see cref="DaemonHostBuilder{TDaemonService, TConfig}"/> class.
    /// </summary>
    ///
    /// <param name="workerConfig"> The worker configuration. </param>
    ///-------------------------------------------------------------------------------------------------
    public DaemonHostBuilder(TConfig workerConfig) : base(workerConfig) { }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Is called to add services to container. Can be overridden in inherited classes. Adds the
    ///     worker config to the container.
    /// </summary>
    ///
    /// <param name="services"> The services. </param>
    ///
    /// <seealso cref="WorkerHostBuilder{DaemonWorker,TConfig}.ConfigureServices(IServiceCollection)"/>
    ///-------------------------------------------------------------------------------------------------
    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
        services.AddSingleton<IDaemonService, TDaemonService>();
    }
}