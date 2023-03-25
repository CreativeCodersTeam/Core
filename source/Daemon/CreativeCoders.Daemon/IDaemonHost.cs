using System.Threading.Tasks;

namespace CreativeCoders.Daemon;

public interface IDaemonHost
{
    /// <summary>
    /// Runs the host asynchronously
    /// </summary>
    /// <returns></returns>
    Task RunAsync();
}
