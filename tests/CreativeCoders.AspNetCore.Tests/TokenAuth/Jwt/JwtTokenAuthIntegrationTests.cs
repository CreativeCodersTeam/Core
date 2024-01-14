using System.Net;
using System.Security.Claims;
using CreativeCoders.AspNetCore.Tests.TokenAuth.Jwt.TestServerSetup;
using CreativeCoders.AspNetCore.TokenAuthApi.Abstractions;
using CreativeCoders.AspNetCore.TokenAuthApi.Api;

namespace CreativeCoders.AspNetCore.Tests.TokenAuth.Jwt;

public sealed class JwtTokenAuthIntegrationTests : IDisposable
{
    private readonly TestServerContext<TestStartup> _testContext;

    private readonly IUserAuthProvider _userAuthProvider;

    private readonly IUserClaimsProvider _userClaimsProvider;

    private readonly ITokenCreator _tokenCreator;

    public JwtTokenAuthIntegrationTests()
    {
        _userAuthProvider = A.Fake<IUserAuthProvider>();
        _userClaimsProvider = A.Fake<IUserClaimsProvider>();
        _tokenCreator = A.Fake<ITokenCreator>();

        _testContext = new TestServerContext<TestStartup>(_userAuthProvider, _userClaimsProvider, _tokenCreator);
    }

    [Fact]
    public async Task Login_EmptyCredentials_ReturnsNotAuthorizedStatusCode()
    {
        // Arrange
        var loginRequest = new LoginRequest();

        // Act
        var response = await _testContext.HttpClient.PostAsync(
            new Uri("api/TokenAuth/login", UriKind.Relative), JsonContent.Create(loginRequest));

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Login_WithCredentials_ReturnsToken()
    {
        const string token = "123456789012";

        // Arrange
        var loginRequest = new LoginRequest
        {
            UserName = "test",
            Password = "password",
            Domain = "example.com"
        };

        A.CallTo(() => _tokenCreator.CreateTokenAsync(A<string>.Ignored, A<string>.Ignored, A<IEnumerable<Claim>>.Ignored))
            .Returns(Task.FromResult(token));

        A.CallTo(() =>
                _userAuthProvider.AuthenticateAsync(loginRequest.UserName, loginRequest.Password,
                    loginRequest.Domain))
            .Returns(Task.FromResult(true));

        // Act
        var response = await _testContext.HttpClient.PostAsync(
            new Uri("api/TokenAuth/login", UriKind.Relative), JsonContent.Create(loginRequest));

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        var cookie = response.Headers.GetValues("set-cookie").First();

        cookie
            .Should()
            .Be($"auth_token={token}; path=/; secure; httponly");
    }

    public void Dispose()
    {
        _testContext.DisposeAsync().AsTask().GetAwaiter().GetResult();
    }
}
