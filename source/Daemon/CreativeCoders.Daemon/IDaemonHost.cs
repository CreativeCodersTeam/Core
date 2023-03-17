namespace CreativeCoders.Daemon;

public interface IDaemonHost
{
    /// <summary>
    /// Runs the host
    /// </summary>
    /// <returns></returns>
    Task RunAsync();
}
