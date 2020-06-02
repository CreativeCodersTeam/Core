using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CreativeCoders.Daemon.Base
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Class for building a host for a specific worker. </summary>
    ///
    /// <typeparam name="TWorker">  Type of the worker. </typeparam>
    ///
    /// <seealso cref="IWorkerHostBuilder"/>
    ///-------------------------------------------------------------------------------------------------
    public class WorkerHostBuilder<TWorker> : IWorkerHostBuilder
        where TWorker : BackgroundService
    {
        /// <inheritdoc />
        public IHost Build(Action<IHostBuilder> configureHostBuilder)
        {
            var hostBuilder = CreateHostBuilder(configureHostBuilder)
                .ConfigureLogging(x => x.SetMinimumLevel(LogLevel.Information))
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<TWorker>();
                    ConfigureServices(services);
                });

            return hostBuilder.Build();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Is called to add services to container. Can be overridden in inherited classes.
        /// </summary>
        ///
        /// <param name="services"> The services. </param>
        ///-------------------------------------------------------------------------------------------------
        protected virtual void ConfigureServices(IServiceCollection services) { }

        private static IHostBuilder CreateHostBuilder(Action<IHostBuilder> setupHostBuilder)
        {
            var hostBuilder = Host.CreateDefaultBuilder();

            setupHostBuilder(hostBuilder);

            return hostBuilder;
        }
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Class for building a host for a specific worker and configuration. </summary>
    ///
    /// <typeparam name="TWorker">  Type of the worker. </typeparam>
    /// <typeparam name="TConfig">  Type of the configuration. </typeparam>
    ///
    /// <seealso cref="WorkerHostBuilder{TWorker}"/>
    ///-------------------------------------------------------------------------------------------------
    public class WorkerHostBuilder<TWorker, TConfig> : WorkerHostBuilder<TWorker>
        where TWorker : BackgroundService
        where TConfig : class
    {
        private readonly TConfig _workerConfig;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the CreativeCoders.Daemon.Base.WorkerHostBuilder&lt;TWorker,
        ///     TConfig&gt; class.
        /// </summary>
        ///
        /// <param name="workerConfig"> The worker configuration. </param>
        ///-------------------------------------------------------------------------------------------------
        public WorkerHostBuilder(TConfig workerConfig)
        {
            _workerConfig = workerConfig;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Is called to add services to container. Can be overridden in inherited classes. Adds the
        ///     worker config to the container.
        /// </summary>
        ///
        /// <param name="services"> The services. </param>
        ///
        /// <seealso cref="WorkerHostBuilder{TWorker}.ConfigureServices(IServiceCollection)"/>
        ///-------------------------------------------------------------------------------------------------
        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_workerConfig);
        }
    }
}