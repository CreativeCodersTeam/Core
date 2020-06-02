using System;
using Microsoft.Extensions.Hosting;

namespace CreativeCoders.Daemon.Base
{
    /// <summary>   Interface for worker host builder. </summary>
    public interface IWorkerHostBuilder
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Builds the daemon service worker host. </summary>
        ///
        /// <param name="configureHostBuilder"> <see cref="Action"/> which is used for configuring the
        ///                                     <see cref="IHostBuilder"/>. </param>
        ///
        /// <returns>   The daemon service worker <see cref="IHost"/>. </returns>
        ///-------------------------------------------------------------------------------------------------
        IHost Build(Action<IHostBuilder> configureHostBuilder);
    }
}