using Microsoft.AspNetCore.TestHost;

namespace CreativeCoders.AspNetCore.Tests.TokenAuth.Jwt;

public sealed class TestServerContext<TStartup> : IAsyncDisposable
    where TStartup : class
{
    private readonly TestServer _testServer;

    public TestServerContext(Action<IServiceCollection>? configureServices = null)
    {
        var webHostBuilder = SetupWebHostBuilder(configureServices);

        _testServer = new TestServer(webHostBuilder);

        HttpClient = _testServer.CreateClient();
    }

    public HttpClient HttpClient { get; }

    public async ValueTask DisposeAsync()
    {
        await _testServer.Host.StopAsync();

        _testServer.Dispose();
    }

    private IWebHostBuilder SetupWebHostBuilder(Action<IServiceCollection>? configureServices)
    {
        return new WebHostBuilder()
            .ConfigureServices(services => configureServices?.Invoke(services))
            .UseStartup<TStartup>();
    }
}
