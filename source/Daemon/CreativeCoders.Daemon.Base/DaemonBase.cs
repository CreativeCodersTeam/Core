using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Hosting;

namespace CreativeCoders.Daemon.Base
{
    /// <summary>   An abstract base class for a daemon. </summary>
    public abstract class DaemonBase
    {
        private readonly IWorkerHostBuilder _hostBuilder;

        private readonly string _installArg;

        private readonly string _uninstallArg;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the CreativeCoders.Daemon.Base.DaemonBase class.
        /// </summary>
        ///
        /// <param name="hostBuilder">  The host builder. </param>
        /// <param name="installArg">   The install argument. </param>
        /// <param name="uninstallArg"> The uninstall argument. </param>
        ///-------------------------------------------------------------------------------------------------
        protected DaemonBase(IWorkerHostBuilder hostBuilder, string installArg, string uninstallArg)
        {
            _hostBuilder = hostBuilder;
            _installArg = installArg;
            _uninstallArg = uninstallArg;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the CreativeCoders.Daemon.Base.DaemonBase class.
        /// </summary>
        ///
        /// <param name="hostBuilder">  The host builder. </param>
        ///-------------------------------------------------------------------------------------------------
        protected DaemonBase(IWorkerHostBuilder hostBuilder)
            : this(hostBuilder, "--install", "--uninstall")
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Runs the daemon asynchronous. </summary>
        ///
        /// <param name="args"> The arguments. </param>
        ///
        /// <returns>   The <see cref="Task"/> that represents the asynchronous operation. </returns>
        ///-------------------------------------------------------------------------------------------------
        public async Task RunAsync(IReadOnlyCollection<string> args)
        {
            if (args.Contains(_installArg))
            {
                InstallDaemon();

                return;
            }

            if (args.Contains(_uninstallArg))
            {
                UninstallDaemon();

                return;
            }

            var host = _hostBuilder.Build(ConfigureHostBuilder);

            await host.RunAsync();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Used to configure the host builder. </summary>
        ///
        /// <param name="hostBuilder">  The host builder. </param>
        ///-------------------------------------------------------------------------------------------------
        protected abstract void ConfigureHostBuilder(IHostBuilder hostBuilder);

        /// <summary>   Installs the daemon into the system. </summary>
        protected abstract void InstallDaemon();

        /// <summary>   Uninstall daemon from the system. </summary>
        protected abstract void UninstallDaemon();
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     An abstract base class for a daemon with an specific daemon service
    ///     <typeparamref name="TDaemonService"/>.
    /// </summary>
    ///
    /// <typeparam name="TDaemonService">   Type of the daemon service. </typeparam>
    ///
    /// <seealso cref="DaemonBase"/>
    ///-------------------------------------------------------------------------------------------------
    [PublicAPI]
    public abstract class DaemonBase<TDaemonService> : DaemonBase
        where TDaemonService : class, IDaemonService
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="DaemonBase{TDaemonService}"/> class.
        /// </summary>
        ///
        /// <param name="installArg">   The install argument. </param>
        /// <param name="uninstallArg"> The uninstall argument. </param>
        ///-------------------------------------------------------------------------------------------------
        protected DaemonBase(string installArg, string uninstallArg)
            : base(new DaemonHostBuilder<TDaemonService>(), installArg, uninstallArg) { }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="DaemonBase{TDaemonService}"/> class.
        /// </summary>
        ///-------------------------------------------------------------------------------------------------
        protected DaemonBase() : base(new DaemonHostBuilder<TDaemonService>()) { }
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     An abstract base class for a daemon with an specific daemon service
    ///     <typeparamref name="TDaemonService"/> and a configuration <typeparamref name="TConfig"/>.
    /// </summary>
    ///
    /// <typeparam name="TDaemonService">   Type of the daemon service. </typeparam>
    /// <typeparam name="TConfig">          Type of the configuration. </typeparam>
    ///
    /// <seealso cref="DaemonBase"/>
    ///-------------------------------------------------------------------------------------------------
    [PublicAPI]
    public abstract class DaemonBase<TDaemonService, TConfig> : DaemonBase
        where TDaemonService : class, IDaemonService
        where TConfig : class
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="DaemonBase{TDaemonService, TConfig}"/> class.
        /// </summary>
        ///
        /// <param name="workerConfig"> The worker configuration. </param>
        /// <param name="installArg">   The install argument. </param>
        /// <param name="uninstallArg"> The uninstall argument. </param>
        ///-------------------------------------------------------------------------------------------------
        protected DaemonBase(TConfig workerConfig, string installArg, string uninstallArg)
            : base(new DaemonHostBuilder<TDaemonService, TConfig>(workerConfig), installArg, uninstallArg) { }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="DaemonBase{TDaemonService, TConfig}"/> class.
        /// </summary>
        ///
        /// <param name="workerConfig"> The worker configuration. </param>
        ///-------------------------------------------------------------------------------------------------
        protected DaemonBase(TConfig workerConfig)
            : base(new DaemonHostBuilder<TDaemonService, TConfig>(workerConfig)) { }
    }
}