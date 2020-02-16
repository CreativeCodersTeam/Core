using System;
using System.Linq;
using System.Threading.Tasks;
using CreativeCoders.Core;
using JetBrains.Annotations;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CreativeCoders.Net.Servers.Http.AspNetCore
{
    [PublicAPI]
    public class AspNetCoreWebHost : IDisposable
    {
        private IWebHost _webHost;

        private readonly Func<HttpContext, Task> _handleRequest;

        public AspNetCoreWebHost(Func<HttpContext, Task> handleRequest)
        {
            Ensure.IsNotNull(handleRequest, nameof(handleRequest));
            
            _handleRequest = handleRequest;
        }

        public Task StartAsync(IWebHostConfig webHostConfig)
        {
            Ensure.IsNotNull(webHostConfig, nameof(webHostConfig));

            var webHostBuilder = CreateWebHostBuilder();
            if (webHostConfig.DisableLogging)
            {
                webHostBuilder = webHostBuilder.ConfigureLogging(x => x.ClearProviders());
            }
            
            _webHost = webHostBuilder
                .UseUrls(webHostConfig.Urls.ToArray())
                .Build();

            return _webHost.StartAsync();
        }

        public Task StopAsync()
        {
            return _webHost.StopAsync();
        }

        public IWebHostBuilder CreateWebHostBuilder() =>
            WebHost.CreateDefaultBuilder()
                .Configure(app => app.Run(async context => await _handleRequest(context).ConfigureAwait(false)));

        public void Dispose()
        {
            _webHost?.Dispose();
        }
    }
}