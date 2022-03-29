using System;
using System.Threading.Tasks;
using CreativeCoders.Net.Http.Auth;
using Xunit;

namespace CreativeCoders.Net.UnitTests.Http.Auth;

public class NullHttpClientAuthenticatorTests
{
    [Fact]
    public void CanAuthenticate_Call_AlwaysReturnsFalse()
    {
        var authenticator = new NullHttpClientAuthenticator();

        Assert.False(authenticator.CanAuthenticate(new Uri("http://test.com")));
        Assert.False(authenticator.CanAuthenticate(new Uri("http://test.com")));
    }

    [Fact]
    public async Task AuthenticateAsync_Call_ThrowsException()
    {
        var authenticator = new NullHttpClientAuthenticator();

        await Assert.ThrowsAsync<NotSupportedException>(() =>
            authenticator.AuthenticateAsync(new Uri("http://test.com")));
    }

    [Fact]
    public void Default_Get_AlwaysReturnsSameInstance()
    {
        var instance0 = NullHttpClientAuthenticator.Default;
        var instance1 = NullHttpClientAuthenticator.Default;

        Assert.Same(instance0, instance1);
    }

    [Fact]
    public void PrepareRequest_Call_DoesNothing()
    {
        var authenticator = new NullHttpClientAuthenticator();

        authenticator.PrepareHttpRequest(null);
    }

    [Fact]
    public void CanAuthenticate_Call_ReturnsFalse()
    {
        var authenticator = new NullHttpClientAuthenticator();

        Assert.False(authenticator.CanAuthenticate(new Uri("http://test.com")));
    }
}