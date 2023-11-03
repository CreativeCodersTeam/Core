using System.Net;
using System.Threading.Tasks;
using CreativeCoders.UnitTests.Net.Http;
using Xunit;

namespace CreativeCoders.Net.UnitTests.UnitTests.Net.Http;

public class MockHttpClientContextTests
{
    [Fact]
    public async Task CreateClient_NoResponderDefined_ThrowsException()
    {
        var context = new MockHttpClientContext();

        var client = context.CreateClient();

        await Assert.ThrowsAsync<NoResponderFoundException>(() => client.GetStringAsync("http://test.com"))
            .ConfigureAwait(false);
    }

    [Fact]
    public async Task CreateClient_WithResponder_ReturnsCorrectContent()
    {
        const string expectedContent = "TestData";

        var context = new MockHttpClientContext();

        context
            .Respond()
            .ForUri("http://test.com/")
            .ReturnText(expectedContent, HttpStatusCode.OK);

        var client = context.CreateClient();

        var response = await client.GetStringAsync("http://test.com").ConfigureAwait(false);

        Assert.Equal(expectedContent, response);
    }

    [Fact]
    public async Task CreateClient_WithMultipleResponder_ReturnsCorrectContent()
    {
        const string expectedContent = "TestData";
        const string expectedSecondContent = "1234";

        var context = new MockHttpClientContext();

        context
            .Respond()
            .ForUri("http://test.com/")
            .ReturnText(expectedContent, HttpStatusCode.OK);

        context
            .Respond()
            .ForUri("http://nic.com/")
            .ReturnText(expectedSecondContent, HttpStatusCode.OK);

        var client = context.CreateClient();

        var response = await client.GetStringAsync("http://test.com").ConfigureAwait(false);

        Assert.Equal(expectedContent, response);

        var secondResponse = await client.GetStringAsync("http://nic.com").ConfigureAwait(false);

        Assert.Equal(expectedSecondContent, secondResponse);
    }
}
