namespace CreativeCoders.Daemon;

public interface IDaemonService
{
    Task StartAsync();

    Task StopAsync();
}
