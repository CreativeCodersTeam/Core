using Microsoft.Extensions.Hosting;

namespace CreativeCoders.Daemon;

internal class DaemonWorker : BackgroundService
{
    private readonly IDaemonService _daemonService;

    public DaemonWorker(IDaemonService daemonService)
    {
        _daemonService = daemonService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _daemonService.StartAsync().ConfigureAwait(false);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken).ConfigureAwait(false);
        }

        await _daemonService.StopAsync().ConfigureAwait(false);
    }
}
