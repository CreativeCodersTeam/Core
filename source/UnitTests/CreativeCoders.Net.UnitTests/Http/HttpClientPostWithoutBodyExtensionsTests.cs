using System;
using System.Threading.Tasks;
using CreativeCoders.Net.Http;
using CreativeCoders.UnitTests.Net.Http;
using Xunit;

namespace CreativeCoders.Net.UnitTests.Http
{
    public class HttpClientPostWithoutBodyExtensionsTests
    {
        [Fact]
        public async Task PostAsync_WithDefaultParameters_PostRequestIsMadeWithOutContent()
        {
            var mockHttpClientContext = new MockHttpClientContext();

            mockHttpClientContext
                .Respond()
                .ReturnText("Test");

            var client = mockHttpClientContext.CreateClient();

            var _ = await client.PostAsync(new Uri("http://test.com"));

            Assert.Single(mockHttpClientContext.RecordedRequests);

            mockHttpClientContext
                .CallShouldBeMade("http://test.com/")
                .WithContentText(string.Empty);
        }
    }
}