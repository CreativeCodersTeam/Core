using System.Threading.Tasks;
using CreativeCoders.Daemon.Base;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace WindowsServiceDaemonSampleApp;

[UsedImplicitly]
public class SampleDaemonService : IDaemonService
{
    // ReSharper disable once SuggestBaseTypeForParameter
    public SampleDaemonService(ILogger<SampleDaemonService> logger, SampleWorkerConfig workerConfig)
    {
        logger.LogInformation($"SampleDaemonService created. TestData = {workerConfig.TestData}");
    }

    public Task StartAsync()
    {
        return Task.CompletedTask;
    }

    public Task StopAsync()
    {
        return Task.CompletedTask;
    }
}
