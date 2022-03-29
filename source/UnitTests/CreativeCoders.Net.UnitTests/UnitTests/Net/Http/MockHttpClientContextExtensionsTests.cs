using System.Net;
using System.Threading.Tasks;
using CreativeCoders.UnitTests.Net.Http;
using Xunit;

namespace CreativeCoders.Net.UnitTests.UnitTests.Net.Http;

public class MockHttpClientContextExtensionsTests
{
    [Fact]
    public void CallShouldBeMade_NoRecordedRequests_ThrowsException()
    {
        var context = new MockHttpClientContext();

        Assert.Throws<RecordedRequestVerificationFailedException>(
            () => context.CallShouldBeMade("http://test.com/"));
    }

    [Fact]
    public async Task CallShouldBeMade_OneRecordedRequest_PassesWithoutException()
    {
        var context = new MockHttpClientContext();

        context
            .Respond()
            .ReturnText("TestData", HttpStatusCode.OK);

        var client = context.CreateClient();

        var response = await client.GetStringAsync("http://test.com").ConfigureAwait(false);

        Assert.Equal("TestData", response);

        context.CallShouldBeMade("http://test.com/");
    }

    [Fact]
    public async Task CallShouldBeMade_OneRecordedRequestNotMatchingUri_PassesWithoutException()
    {
        var context = new MockHttpClientContext();

        context
            .Respond()
            .ReturnText("TestData", HttpStatusCode.OK);

        var client = context.CreateClient();

        var response = await client.GetStringAsync("http://test1.com").ConfigureAwait(false);

        Assert.Equal("TestData", response);

        Assert.Throws<RecordedRequestVerificationFailedException>(
            () => context.CallShouldBeMade("http://test.com/"));
    }
}