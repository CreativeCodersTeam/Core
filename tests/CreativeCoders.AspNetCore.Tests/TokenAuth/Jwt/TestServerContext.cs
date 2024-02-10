using CreativeCoders.AspNetCore.TokenAuthApi;
using CreativeCoders.AspNetCore.TokenAuthApi.Abstractions;
using CreativeCoders.AspNetCore.TokenAuthApi.Jwt;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CreativeCoders.AspNetCore.Tests.TokenAuth.Jwt;

public sealed class TestServerContext<TStartup> : IAsyncDisposable
    where TStartup : class
{
    private readonly TestServer _testServer;

    public TestServerContext(Action<IServiceCollection>? configureServices = null,
        Action<JwtTokenAuthApiOptions>? configureJwtApiOptions = null,
        Action<TokenAuthApiOptions>? configureTokenApiOptions = null)
    {
        UserClaimsProvider = A.Fake<IUserClaimsProvider>();
        UserAuthProvider = A.Fake<IUserAuthProvider>();
        TokenCreator = A.Fake<ITokenCreator>();

        var webHostBuilder = SetupWebHostBuilder(services =>
        {
            configureServices?.Invoke(services);

            services.TryAddScoped(_ => UserClaimsProvider);
            services.TryAddScoped(_ => UserAuthProvider);
            services.TryAddScoped(_ => TokenCreator);

            services.AddJwtTokenAuthApi(configureJwtApiOptions, configureTokenApiOptions);
        });

        _testServer = new TestServer(webHostBuilder);

        HttpClient = _testServer.CreateClient();
    }

    public HttpClient HttpClient { get; }

    public ITokenCreator TokenCreator { get; }

    public IUserAuthProvider UserAuthProvider { get; }

    public IUserClaimsProvider UserClaimsProvider { get; }

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
