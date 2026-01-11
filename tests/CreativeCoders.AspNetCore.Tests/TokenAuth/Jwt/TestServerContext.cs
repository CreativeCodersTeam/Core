using System.Text;
using CreativeCoders.AspNetCore.TokenAuth.Jwt;
using CreativeCoders.AspNetCore.TokenAuthApi;
using CreativeCoders.AspNetCore.TokenAuthApi.Abstractions;
using CreativeCoders.AspNetCore.TokenAuthApi.Jwt;
using CreativeCoders.Core.Text;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;

namespace CreativeCoders.AspNetCore.Tests.TokenAuth.Jwt;

public sealed class TestServerContext<TStartup> : IAsyncDisposable
    where TStartup : class
{
    private readonly TestServer _testServer;

    private readonly IHost _host;

    public TestServerContext(Action<IServiceCollection>? configureServices = null,
        Action<JwtTokenAuthApiOptions>? configureJwtApiOptions = null,
        Action<TokenAuthApiOptions>? configureTokenApiOptions = null,
        Action<JwtAuthenticationOptions>? configureJwtAuthOptions = null)
    {
        UserClaimsProvider = A.Fake<IUserClaimsProvider>();
        UserAuthProvider = A.Fake<IUserAuthProvider>();
        TokenCreator = A.Fake<ITokenCreator>();

        var webHostBuilder = SetupWebHostBuilder(services =>
        {
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddDebug();
                loggingBuilder.SetMinimumLevel(LogLevel.Information);
            });

            configureServices?.Invoke(services);

            services.TryAddScoped(_ => UserClaimsProvider);
            services.TryAddScoped(_ => UserAuthProvider);
            services.TryAddScoped(_ => TokenCreator);

            services.AddJwtTokenAuthApi(configureJwtApiOptions, configureTokenApiOptions);

            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(RandomString.Create()));
            services.Configure<JwtTokenAuthApiOptions>(x => { x.SecurityKey = securityKey; });

            services.AddJwtTokenAuthentication(x =>
            {
                x.SecurityKey = securityKey;

                configureJwtAuthOptions?.Invoke(x);
            });
        });

        _host = webHostBuilder.Build();

        _host.StartAsync().Wait();

        _testServer = _host.GetTestServer();

        HttpClient = _testServer.CreateClient();
    }

    public HttpClient HttpClient { get; }

    public ITokenCreator TokenCreator { get; }

    public IUserAuthProvider UserAuthProvider { get; }

    public IUserClaimsProvider UserClaimsProvider { get; }

    public async ValueTask DisposeAsync()
    {
        await _host.StopAsync();
        _host.Dispose();
    }

    private static IHostBuilder SetupWebHostBuilder(Action<IServiceCollection>? configureServices)
    {
        return new HostBuilder()
            .ConfigureWebHost(webHostBuilder =>
            {
                webHostBuilder
                    .ConfigureServices(services => configureServices?.Invoke(services))
                    .UseTestServer() // If using TestServer
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseStartup<TStartup>();
            });
    }
}
