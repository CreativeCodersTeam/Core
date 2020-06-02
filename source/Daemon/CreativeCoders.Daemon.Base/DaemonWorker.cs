using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace CreativeCoders.Daemon.Base
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   The default worker for a daemon service. </summary>
    ///
    /// <seealso cref="BackgroundService"/>
    ///-------------------------------------------------------------------------------------------------
    public class DaemonWorker : BackgroundService
    {
        private readonly IDaemonService _daemonService;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="DaemonWorker"/> class.
        /// </summary>
        ///
        /// <param name="daemonService">    The daemon service. </param>
        ///-------------------------------------------------------------------------------------------------
        public DaemonWorker(IDaemonService daemonService)
        {
            _daemonService = daemonService;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     This method is called when the
        ///     <see cref="T:Microsoft.Extensions.Hosting.IHostedService" /> starts. Starts the daemon
        ///     service asynchronous, waits for cancellation and stops the daemon service asynchronous.
        /// </summary>
        ///
        /// <param name="stoppingToken">    Triggered when
        ///                                 <see cref="M:Microsoft.Extensions.Hosting.IHostedService.StopAsync(System.Threading.CancellationToken)" />
        ///                                 is called. </param>
        ///
        /// <returns>
        ///     A <see cref="T:System.Threading.Tasks.Task" /> that represents the long running
        ///     operations.
        /// </returns>
        ///
        /// <seealso cref="Microsoft.Extensions.Hosting.BackgroundService.ExecuteAsync(CancellationToken)"/>
        ///-------------------------------------------------------------------------------------------------
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _daemonService.StartAsync();

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }

            await _daemonService.StopAsync();
        }
    }
}