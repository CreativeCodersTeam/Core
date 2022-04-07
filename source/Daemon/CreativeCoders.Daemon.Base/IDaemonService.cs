using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CreativeCoders.Daemon.Base;

/// <summary>   Interface for daemon service implementation. </summary>
[PublicAPI]
public interface IDaemonService
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Starts the daemon server asynchronous. </summary>
    ///
    /// <returns>   The <see cref="Task"/> that represents the asynchronous operation. </returns>
    ///-------------------------------------------------------------------------------------------------
    Task StartAsync();

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Stops the daemon service asynchronous. </summary>
    ///
    /// <returns>   The <see cref="Task"/> that represents the asynchronous operation. </returns>
    ///-------------------------------------------------------------------------------------------------
    Task StopAsync();
}
