using CreativeCoders.Daemon.Base;
using JetBrains.Annotations;
using Microsoft.Extensions.Hosting;

namespace CreativeCoders.Daemon.Linux;

///-------------------------------------------------------------------------------------------------
/// <summary>   A systemd based daemon. </summary>
///
/// <seealso cref="DaemonBase"/>
///-------------------------------------------------------------------------------------------------
[PublicAPI]
public class SystemdDaemon : DaemonBase
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Initializes a new instance of the <see cref="SystemdDaemon"/> class.
    /// </summary>
    ///
    /// <param name="hostBuilder">  The host builder. </param>
    /// <param name="installArg">   The install argument. </param>
    /// <param name="uninstallArg"> The uninstall argument. </param>
    ///-------------------------------------------------------------------------------------------------
    public SystemdDaemon(IWorkerHostBuilder hostBuilder, string installArg, string uninstallArg)
        : base(hostBuilder, installArg, uninstallArg) { }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Initializes a new instance of the <see cref="SystemdDaemon"/> class. </summary>
    ///
    /// <param name="hostBuilder">  The host builder. </param>
    ///-------------------------------------------------------------------------------------------------
    public SystemdDaemon(IWorkerHostBuilder hostBuilder) : base(hostBuilder) { }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Used to configure the host builder. Configures the <paramref name="hostBuilder"/> to use
    ///     systemd lifecycle.
    /// </summary>
    ///
    /// <param name="hostBuilder">  The host builder. </param>
    ///
    /// <seealso cref="DaemonBase.ConfigureHostBuilder(IHostBuilder)"/>
    ///-------------------------------------------------------------------------------------------------
    protected override void ConfigureHostBuilder(IHostBuilder hostBuilder)
    {
        hostBuilder.UseSystemd();
    }

    /// <inheritdoc/>
    protected override void InstallDaemon()
    {
        new SystemdDaemonInstaller().Install();
    }

    /// <inheritdoc/>
    protected override void UninstallDaemon()
    {
        new SystemdDaemonInstaller().Uninstall();
    }
}

///-------------------------------------------------------------------------------------------------
/// <summary>   A systemd based daemon. </summary>
///
/// <typeparam name="TDaemonService">   Type of the daemon service. </typeparam>
///
/// <seealso cref="SystemdDaemon"/>
///-------------------------------------------------------------------------------------------------
[PublicAPI]
public class SystemdDaemon<TDaemonService> : SystemdDaemon
    where TDaemonService : class, IDaemonService
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Initializes a new instance of the <see cref="SystemdDaemon{TDaemonService}"/> class.
    /// </summary>
    ///
    /// <param name="installArg">   The install argument. </param>
    /// <param name="uninstallArg"> The uninstall argument. </param>
    ///-------------------------------------------------------------------------------------------------
    public SystemdDaemon(string installArg, string uninstallArg)
        : base(new DaemonHostBuilder<TDaemonService>(), installArg, uninstallArg) { }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Initializes a new instance of the <see cref="SystemdDaemon{TDaemonService}"/> class.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public SystemdDaemon() : base(new DaemonHostBuilder<TDaemonService>()) { }
}

///-------------------------------------------------------------------------------------------------
/// <summary>   A systemd based daemon. </summary>
///
/// <typeparam name="TDaemonService">   Type of the daemon service. </typeparam>
/// <typeparam name="TConfig">          Type of the configuration. </typeparam>
///
/// <seealso cref="SystemdDaemon"/>
///-------------------------------------------------------------------------------------------------
[PublicAPI]
public class SystemdDaemon<TDaemonService, TConfig> : SystemdDaemon
    where TDaemonService : class, IDaemonService
    where TConfig : class
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Initializes a new instance of the <see cref="SystemdDaemon{TDaemonService, TConfig}"/> class.
    /// </summary>
    ///
    /// <param name="workerConfig"> The worker configuration. </param>
    /// <param name="installArg">   The install argument. </param>
    /// <param name="uninstallArg"> The uninstall argument. </param>
    ///-------------------------------------------------------------------------------------------------
    public SystemdDaemon(TConfig workerConfig, string installArg, string uninstallArg)
        : base(new DaemonHostBuilder<TDaemonService, TConfig>(workerConfig), installArg, uninstallArg) { }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Initializes a new instance of the <see cref="SystemdDaemon{TDaemonService, TConfig}"/>
    ///     class.
    /// </summary>
    ///
    /// <param name="workerConfig"> The worker configuration. </param>
    ///-------------------------------------------------------------------------------------------------
    public SystemdDaemon(TConfig workerConfig)
        : base(new DaemonHostBuilder<TDaemonService, TConfig>(workerConfig)) { }
}