using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CreativeCoders.Net.Http.Auth.Jwt;
using FakeItEasy;
using Xunit;

namespace CreativeCoders.Net.UnitTests.Http.Auth.Jwt;

public class JwtHttpClientAuthenticatorTests
{
    [Fact]
    public void CanAuthenticate_CredentialsIsNull_ReturnsFalse()
    {
        var jwtClientMock = A.Fake<IJwtClient>();

        var jwtHttpClientAuthenticator = new JwtHttpClientAuthenticator(jwtClientMock);

        Assert.False(jwtHttpClientAuthenticator.CanAuthenticate(new Uri("http://test.com")));
    }

    [Fact]
    public void CanAuthenticate_CredentialsUserNameNull_ReturnsFalse()
    {
        var jwtClientMock = A.Fake<IJwtClient>();

        var jwtHttpClientAuthenticator = new JwtHttpClientAuthenticator(jwtClientMock)
        {
            Credentials = new NetworkCredential(null, "pass"),
            TokenRequestUri = new Uri("http://test.com")
        };

        Assert.False(jwtHttpClientAuthenticator.CanAuthenticate(new Uri("http://test.com")));
    }

    [Fact]
    public void CanAuthenticate_CredentialsPasswordNull_ReturnsFalse()
    {
        var jwtClientMock = A.Fake<IJwtClient>();

        var jwtHttpClientAuthenticator = new JwtHttpClientAuthenticator(jwtClientMock)
        {
            Credentials = new NetworkCredential("user", (string) null),
            TokenRequestUri = new Uri("http://test.com")
        };

        Assert.False(jwtHttpClientAuthenticator.CanAuthenticate(new Uri("http://test.com")));
    }

    [Fact]
    public void CanAuthenticate_CredentialsNotNullTokenRequestUriIsNull_ReturnsFalse()
    {
        var jwtClientMock = A.Fake<IJwtClient>();

        var jwtHttpClientAuthenticator = new JwtHttpClientAuthenticator(jwtClientMock)
        {
            Credentials = new NetworkCredential("user", "pass")
        };

        Assert.False(jwtHttpClientAuthenticator.CanAuthenticate(new Uri("http://test.com")));
    }

    [Fact]
    public void CanAuthenticate_CredentialsNotNull_ReturnsTrue()
    {
        var jwtClientMock = A.Fake<IJwtClient>();

        var jwtHttpClientAuthenticator = new JwtHttpClientAuthenticator(jwtClientMock)
        {
            Credentials = new NetworkCredential("user", "pass"),
            TokenRequestUri = new Uri("http://test.com")
        };

        Assert.True(jwtHttpClientAuthenticator.CanAuthenticate(new Uri("http://test.com")));
    }

    [Fact]
    public async Task AuthenticateAsync_TokenRequestedAndPrepareRequest_RequestAuthHeaderIsSetCorrect()
    {
        const string expectedToken = "TokenData";

        var jwtClientMock = A.Fake<IJwtClient>();

        A.CallTo(() =>
                jwtClientMock.RequestTokenInfoAsync(new Uri("http://authtest.com/auth"),
                    A<JwtTokenRequest>.Ignored))
            .Returns(Task.FromResult(new JwtTokenInfo(expectedToken)));

        var jwtHttpClientAuthenticator = new JwtHttpClientAuthenticator(jwtClientMock)
        {
            Credentials = new NetworkCredential("user", "pass"),
            TokenRequestUri = new Uri("http://authtest.com/auth")
        };

        await jwtHttpClientAuthenticator.AuthenticateAsync(new Uri("http://test.com"));

        var request = new HttpRequestMessage();

        jwtHttpClientAuthenticator.PrepareHttpRequest(request);

        var authHeader = request.Headers.Authorization;

        Assert.NotNull(authHeader);
        Assert.Equal("Bearer", authHeader.Scheme);
        Assert.Equal(expectedToken, authHeader.Parameter);
    }

    [Fact]
    public async Task AuthenticateAsync_CanAuthenticateFalse_JwtClientIsNotCalled()
    {
        const string expectedToken = "TokenData";

        var jwtClientMock = A.Fake<IJwtClient>();

        A.CallTo(() => jwtClientMock.RequestTokenInfoAsync(new Uri("http://authtest.com/auth"),
                A<JwtTokenRequest>.Ignored))
            .Returns(Task.FromResult(new JwtTokenInfo(expectedToken)));

        var jwtHttpClientAuthenticator = new JwtHttpClientAuthenticator(jwtClientMock);

        await jwtHttpClientAuthenticator.AuthenticateAsync(new Uri("http://test.com"));

        A.CallTo(() => jwtClientMock.RequestTokenInfoAsync(A<Uri>.Ignored, A<JwtTokenRequest>.Ignored))
            .MustNotHaveHappened();
    }

    [Fact]
    public void PrepareHttpRequest_NoToken_AuthHeaderOnRequestIsNotSet()
    {
        var jwtClientMock = A.Fake<IJwtClient>();

        var jwtHttpClientAuthenticator = new JwtHttpClientAuthenticator(jwtClientMock);

        var request = new HttpRequestMessage();

        jwtHttpClientAuthenticator.PrepareHttpRequest(request);

        Assert.Null(request.Headers.Authorization);
    }
}
