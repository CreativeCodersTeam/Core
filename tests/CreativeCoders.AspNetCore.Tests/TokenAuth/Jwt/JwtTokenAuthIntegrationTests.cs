using System.Net;
using CreativeCoders.AspNetCore.Tests.TokenAuth.Jwt.TestServerSetup;
using CreativeCoders.AspNetCore.TokenAuthApi.Abstractions;
using CreativeCoders.AspNetCore.TokenAuthApi.Api;

namespace CreativeCoders.AspNetCore.Tests.TokenAuth.Jwt;

public sealed class JwtTokenAuthIntegrationTests : IDisposable
{
    private readonly TestServerContext<TestStartup> _testContext;

    private readonly IUserAuthProvider _userAuthProvider;

    private readonly IUserClaimsProvider _userClaimsProvider;

    public JwtTokenAuthIntegrationTests()
    {
        _userAuthProvider = A.Fake<IUserAuthProvider>();
        _userClaimsProvider = A.Fake<IUserClaimsProvider>();

        _testContext = new TestServerContext<TestStartup>(_userAuthProvider, _userClaimsProvider);
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
        // Arrange
        var loginRequest = new LoginRequest
        {
            UserName = "test",
            Password = "password",
            Domain = "example.com"
        };

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

        response.Headers.GetValues("set-cookie").First()
            .Should()
            .NotBeNullOrWhiteSpace();
    }

    public void Dispose()
    {
        _testContext.DisposeAsync().AsTask().GetAwaiter().GetResult();
    }
}
