using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CreativeCoders.Core;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CreativeCoders.Net.Servers.Http.AspNetCore
{
    [PublicAPI]
    public class AspNetCoreWebHost : IDisposable
    {
        private IHost _webHost;

        private readonly Func<HttpContext, Task> _handleRequest;

        public AspNetCoreWebHost(Func<HttpContext, Task> handleRequest)
        {
            Ensure.IsNotNull(handleRequest, nameof(handleRequest));
            
            _handleRequest = handleRequest;
        }

        public Task StartAsync(IWebHostConfig webHostConfig)
        {
            Ensure.IsNotNull(webHostConfig, nameof(webHostConfig));

            var webHostBuilder = CreateWebHostBuilder(webHostConfig.Urls);
            if (webHostConfig.DisableLogging)
            {
                webHostBuilder = webHostBuilder.ConfigureLogging(x => x.ClearProviders());
            }
            
            _webHost = webHostBuilder
                .Build();

            return _webHost.StartAsync();
        }

        public async Task StopAsync()
        {
            await _webHost.StopAsync();
            _webHost.Dispose();
        }

        private IHostBuilder CreateWebHostBuilder(IReadOnlyCollection<string> urls)
        {
            var hostBuilder = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.Configure(app =>
                        app.Run(async context => await _handleRequest(context).ConfigureAwait(false)));

                    webBuilder.UseUrls(urls.ToArray());
                });

            return hostBuilder;
        }
            
        public void Dispose()
        {
            _webHost?.Dispose();
        }
    }
}