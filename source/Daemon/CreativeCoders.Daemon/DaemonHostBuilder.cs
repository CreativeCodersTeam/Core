namespace CreativeCoders.Daemon;

public static class DaemonHostBuilder
{
    /// <summary>
    /// Creates the default daemon host builder
    /// </summary>
    /// <typeparam name="TDaemonService">Type of the daemon service which should run</typeparam>
    /// <returns>The instantiated daemon host builder</returns>
    public static IDaemonHostBuilder CreateBuilder<TDaemonService>()
        where TDaemonService : class, IDaemonService
    {
        return new DefaultDaemonHostBuilder<TDaemonService>();
    }
}
