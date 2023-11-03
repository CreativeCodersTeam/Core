using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CreativeCoders.UnitTests.Net.Http;
using Xunit;

namespace CreativeCoders.Net.UnitTests.UnitTests.Net.Http;

[SuppressMessage("ReSharper", "MethodSupportsCancellation")]
public class RecordedRequestVerifierTests
{
    [Fact]
    public async Task WithVerb_CheckForGet_PassesWithoutException()
    {
        var context = new MockHttpClientContext();

        context
            .Respond()
            .ReturnText("TestData");

        var client = context.CreateClient();

        var response = await client.GetStringAsync("http://test.com").ConfigureAwait(false);

        Assert.Equal("TestData", response);

        context
            .CallShouldBeMade("http://test.com/")
            .WithVerb(HttpMethod.Get);
    }

    [Fact]
    public async Task WithVerb_CheckForPost_ThrowsException()
    {
        var context = new MockHttpClientContext();

        context
            .Respond()
            .ReturnText("TestData");

        var client = context.CreateClient();

        var response = await client.GetStringAsync("http://test.com").ConfigureAwait(false);

        Assert.Equal("TestData", response);

        Assert.Throws<RecordedRequestVerificationFailedException>(() =>
            context
                .CallShouldBeMade("http://test.com/")
                .WithVerb(HttpMethod.Post));
    }

    [Fact]
    public async Task WithContentType_CheckForOctetStream_PassesWithoutException()
    {
        var context = new MockHttpClientContext();

        context
            .Respond()
            .ReturnText("TestData");

        var client = context.CreateClient();

        var response = await client.PostAsync("http://test.com",
                new StringContent("Test", Encoding.UTF8, ContentMediaTypes.Application.OctetStream))
            .ConfigureAwait(false);

        Assert.Equal("TestData", await response.Content.ReadAsStringAsync().ConfigureAwait(false));

        context
            .CallShouldBeMade("http://test.com/")
            .WithContentType(ContentMediaTypes.Application.OctetStream);
    }

    [Fact]
    public async Task WithContentType_CheckForTextPlain_ThrowsException()
    {
        var context = new MockHttpClientContext();

        context
            .Respond()
            .ReturnText("TestData");

        var client = context.CreateClient();

        var response = await client.PostAsync("http://test.com",
                new StringContent("Test", Encoding.UTF8, ContentMediaTypes.Application.OctetStream))
            .ConfigureAwait(false);

        Assert.Equal("TestData", await response.Content.ReadAsStringAsync().ConfigureAwait(false));

        Assert.Throws<RecordedRequestVerificationFailedException>(() =>
            context
                .CallShouldBeMade("http://test.com/")
                .WithContentType(ContentMediaTypes.Text.Plain));
    }

    [Fact]
    public async Task RequestMeets_CheckForCancellationToken_PassesWithoutException()
    {
        var cancellationTokenSource = new CancellationTokenSource();
        var expectedCancellationToken = cancellationTokenSource.Token;

        var context = new MockHttpClientContext();

        context
            .Respond()
            .ReturnText("TestData");

        var client = context.CreateClient();

        var response = await client.GetAsync("http://test.com", HttpCompletionOption.ResponseContentRead,
            expectedCancellationToken).ConfigureAwait(false);

        Assert.Equal("TestData", await response.Content.ReadAsStringAsync().ConfigureAwait(false));

        context
            .CallShouldBeMade("http://test.com/")
            .RequestMeets((_, cancellationToken) => !cancellationToken.IsCancellationRequested,
                "IsCancellationRequested is false");
    }

    [Fact]
    public async Task RequestMeets_CheckForCancellationToken_IsCanceled()
    {
        var cancellationTokenSource = new CancellationTokenSource();
        var expectedCancellationToken = cancellationTokenSource.Token;

        var context = new MockHttpClientContext();

        context
            .Respond()
            .ReturnText("TestData");

        var client = context.CreateClient();

        cancellationTokenSource.Cancel();

        var response = await client.GetAsync("http://test.com", HttpCompletionOption.ResponseHeadersRead,
            expectedCancellationToken).ConfigureAwait(false);

        Assert.Equal("TestData", await response.Content.ReadAsStringAsync().ConfigureAwait(false));

        context
            .CallShouldBeMade("http://test.com/")
            .RequestMeets((_, cancellationToken) => cancellationToken.IsCancellationRequested,
                "IsCancellationRequested must be true");
    }

    [Fact]
    public async Task WithContentText_CheckForContentText_PassesWithOutException()
    {
        var context = new MockHttpClientContext();

        context
            .Respond()
            .ReturnText("TestData");

        var client = context.CreateClient();

        var _ = await client.PostAsync("http://test.com",
                new StringContent("Test", Encoding.UTF8, ContentMediaTypes.Application.OctetStream))
            .ConfigureAwait(false);

        context.CallShouldBeMade("*").WithContentText("Test").RequestCount(1);
    }

    [Fact]
    public async Task WithContentText_CheckForWrongContentText_ThrowsException()
    {
        var context = new MockHttpClientContext();

        context
            .Respond()
            .ReturnText("TestData");

        var client = context.CreateClient();

        var _ = await client.PostAsync("http://test.com",
                new StringContent("Test", Encoding.UTF8, ContentMediaTypes.Application.OctetStream))
            .ConfigureAwait(false);

        Assert.Throws<RecordedRequestVerificationFailedException>(() =>
            context.CallShouldBeMade("*").WithContentText("WrongTest"));
    }

    [Fact]
    public async Task RequestCount_OneCallMade_OneRequestCount()
    {
        var context = new MockHttpClientContext();

        context
            .Respond()
            .ReturnText("TestData");

        var client = context.CreateClient();

        var _ = await client.PostAsync("http://test.com",
                new StringContent("Test", Encoding.UTF8, ContentMediaTypes.Application.OctetStream))
            .ConfigureAwait(false);

        context.CallShouldBeMade("*").RequestCount(1);
    }

    [Fact]
    public async Task RequestCount_TwoCallsMadeCheckForTwo_ThrowsException()
    {
        var context = new MockHttpClientContext();

        context
            .Respond()
            .ReturnText("TestData");

        var client = context.CreateClient();

        var _ = await client.PostAsync("http://test.com",
                new StringContent("Test", Encoding.UTF8, ContentMediaTypes.Application.OctetStream))
            .ConfigureAwait(false);

        var __ = await client.PostAsync("http://test.com",
                new StringContent("Test", Encoding.UTF8, ContentMediaTypes.Application.OctetStream))
            .ConfigureAwait(false);

        Assert.Throws<RecordedRequestVerificationFailedException>(() =>
            context.CallShouldBeMade("*").RequestCount(1));
    }
}
