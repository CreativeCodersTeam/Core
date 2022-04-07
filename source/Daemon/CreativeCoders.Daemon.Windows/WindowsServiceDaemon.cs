using CreativeCoders.Daemon.Base;
using JetBrains.Annotations;
using Microsoft.Extensions.Hosting;

namespace CreativeCoders.Daemon.Windows;

///-------------------------------------------------------------------------------------------------
/// <summary>   The windows service daemon. </summary>
///
/// <seealso cref="DaemonBase"/>
///-------------------------------------------------------------------------------------------------
[PublicAPI]
public class WindowsServiceDaemon : DaemonBase
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Initializes a new instance of the <see cref="WindowsServiceDaemon"/> class.
    /// </summary>
    ///
    /// <param name="hostBuilder">  The host builder. </param>
    /// <param name="installArg">   The install argument. </param>
    /// <param name="uninstallArg"> The uninstall argument. </param>
    ///-------------------------------------------------------------------------------------------------
    public WindowsServiceDaemon(IWorkerHostBuilder hostBuilder, string installArg, string uninstallArg)
        : base(hostBuilder, installArg, uninstallArg) { }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Initializes a new instance of the <see cref="WindowsServiceDaemon"/> class.
    /// </summary>
    ///
    /// <param name="hostBuilder">  The host builder. </param>
    ///-------------------------------------------------------------------------------------------------
    public WindowsServiceDaemon(IWorkerHostBuilder hostBuilder)
        : base(hostBuilder) { }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Used to configure the host builder. Configures the <paramref name="hostBuilder"/> to use
    ///     windows service lifecycle.
    /// </summary>
    ///
    /// <param name="hostBuilder">  The host builder. </param>
    ///
    /// <seealso cref="DaemonBase.ConfigureHostBuilder(IHostBuilder)"/>
    ///-------------------------------------------------------------------------------------------------
    protected override void ConfigureHostBuilder(IHostBuilder hostBuilder)
    {
        hostBuilder.UseWindowsService();
    }

    /// <inheritdoc/>
    protected override void InstallDaemon()
    {
        new WindowsServiceInstaller().Install();
    }

    /// <inheritdoc/>
    protected override void UninstallDaemon()
    {
        new WindowsServiceInstaller().Uninstall();
    }
}

///-------------------------------------------------------------------------------------------------
/// <summary>   The windows service daemon. </summary>
///
/// <typeparam name="TDaemonService">   Type of the daemon service. </typeparam>
///
/// <seealso cref="WindowsServiceDaemon"/>
///-------------------------------------------------------------------------------------------------
[PublicAPI]
public class WindowsServiceDaemon<TDaemonService> : WindowsServiceDaemon
    where TDaemonService : class, IDaemonService
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Initializes a new instance of the <see cref="WindowsServiceDaemon{TDaeomService}"/> class.
    /// </summary>
    ///
    /// <param name="installArg">   The install argument. </param>
    /// <param name="uninstallArg"> The uninstall argument. </param>
    ///-------------------------------------------------------------------------------------------------
    public WindowsServiceDaemon(string installArg, string uninstallArg)
        : base(new WindowsDaemonHostBuilder<TDaemonService>(), installArg, uninstallArg) { }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Initializes a new instance of the <see cref="WindowsServiceDaemon{TDaeomService}"/> class.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public WindowsServiceDaemon()
        : base(new WindowsDaemonHostBuilder<TDaemonService>()) { }
}

///-------------------------------------------------------------------------------------------------
/// <summary>   The windows service daemon. </summary>
///
/// <typeparam name="TDaemonService">   Type of the daemon service. </typeparam>
/// <typeparam name="TConfig">          Type of the configuration. </typeparam>
///
/// <seealso cref="WindowsServiceDaemon"/>
///-------------------------------------------------------------------------------------------------
[PublicAPI]
public class WindowsServiceDaemon<TDaemonService, TConfig> : WindowsServiceDaemon
    where TDaemonService : class, IDaemonService
    where TConfig : class
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Initializes a new instance of the
    ///     <see cref="WindowsServiceDaemon{TDaeomService, TConfig}"/> class.
    /// </summary>
    ///
    /// <param name="workerConfig"> The worker configuration. </param>
    /// <param name="installArg">   The install argument. </param>
    /// <param name="uninstallArg"> The uninstall argument. </param>
    ///-------------------------------------------------------------------------------------------------
    public WindowsServiceDaemon(TConfig workerConfig, string installArg, string uninstallArg)
        : base(new WindowsDaemonHostBuilder<TDaemonService, TConfig>(workerConfig), installArg,
            uninstallArg) { }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Initializes a new instance of the
    ///     <see cref="WindowsServiceDaemon{TDaeomService, TConfig}"/> class.
    /// </summary>
    ///
    /// <param name="workerConfig"> The worker configuration. </param>
    ///-------------------------------------------------------------------------------------------------
    public WindowsServiceDaemon(TConfig workerConfig)
        : base(new WindowsDaemonHostBuilder<TDaemonService, TConfig>(workerConfig)) { }
}
