using CreativeCoders.AspNetCore.Tests.TokenAuth.Jwt.TestServerSetup;
using CreativeCoders.AspNetCore.TokenAuthApi.Abstractions;
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

    public TestServerContext(IUserAuthProvider userAuthProvider, IUserClaimsProvider userClaimsProvider)
        : this(services =>
        {
            services.AddScoped<IUserAuthProvider>(_ => new TestUserAuthProvider(userAuthProvider));
            services.AddScoped<IUserClaimsProvider>(_ => new TestUserClaimsProvider(userClaimsProvider));
        })
    {

    }

    private IWebHostBuilder SetupWebHostBuilder(Action<IServiceCollection>? configureServices)
    {
        return new WebHostBuilder()
            .ConfigureServices(services => configureServices?.Invoke(services))
            .UseStartup<TStartup>();
        //WebApplication.CreateBuilder().Build()

        return WebApplication.CreateBuilder().WebHost
            .ConfigureServices(services => configureServices?.Invoke(services))
            .UseStartup<TStartup>();
    }

    public async ValueTask DisposeAsync()
    {
        await _testServer.Host.StopAsync();

        _testServer.Dispose();
    }

    public HttpClient HttpClient { get; }
}
