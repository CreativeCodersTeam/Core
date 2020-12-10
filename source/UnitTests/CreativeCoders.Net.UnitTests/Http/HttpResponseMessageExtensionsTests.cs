using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CreativeCoders.Net.Http;
using CreativeCoders.UnitTests.Net.Http;
using Xunit;

namespace CreativeCoders.Net.UnitTests.Http
{
    public class HttpResponseMessageExtensionsTests
    {
        [Fact]
        public async Task ReceiveTextAsync_GetText_ReturnsTestData()
        {
            const string expectedData = "TestData";

            var mockHttpClientContext = new MockHttpClientContext();
            mockHttpClientContext
                .Respond()
                .ReturnText(expectedData);

            var client = mockHttpClientContext.CreateClient();

            var result = await client
                .GetAsync(new Uri("http://test.com"))
                .ReceiveTextAsync();

            Assert.Single(mockHttpClientContext.RecordedRequests);
            Assert.Equal(expectedData, result);
        }

        [Fact]
        public async Task ReceiveTextAsync_GetTextWith401_ThrowsException()
        {
            const string expectedData = "TestData";

            var mockHttpClientContext = new MockHttpClientContext();
            mockHttpClientContext
                .Respond()
                .ReturnText(expectedData, HttpStatusCode.Unauthorized);

            var client = mockHttpClientContext.CreateClient();

            await Assert.ThrowsAsync<HttpRequestException>(() => client
                .GetAsync(new Uri("http://test.com"))
                .ReceiveTextAsync());
        }

        [Fact]
        public async Task ReceiveJsonAsync_GetJson_ReturnsTestData()
        {
            var expectedData = new HttpTestDataObject
            {
                TestProperty = "TestValue"
            };

            var mockHttpClientContext = new MockHttpClientContext();
            mockHttpClientContext
                .Respond()
                .ReturnJson(expectedData);

            var client = mockHttpClientContext.CreateClient();

            var result = await client
                .GetAsync(new Uri("http://test.com"))
                .ReceiveJsonAsync<HttpTestDataObject>();

            Assert.Single(mockHttpClientContext.RecordedRequests);
            Assert.Equal(expectedData.TestProperty, result.TestProperty);
        }

        [Fact]
        public async Task ReceiveJsonAsync_GetJsonWith401_ThrowsException()
        {
            var expectedData = new HttpTestDataObject
            {
                TestProperty = "TestValue"
            };

            var mockHttpClientContext = new MockHttpClientContext();
            mockHttpClientContext
                .Respond()
                .ReturnJson(expectedData, HttpStatusCode.Unauthorized);

            var client = mockHttpClientContext.CreateClient();

            await Assert.ThrowsAsync<HttpRequestException>(() => client
                .GetAsync(new Uri("http://test.com"))
                .ReceiveJsonAsync<HttpTestDataObject>());
        }

        [Fact]
        public async Task ReceiveJsonAsync_GetJsonObject_ReturnsTestDataAsObject()
        {
            var expectedData = new HttpTestDataObject
            {
                TestProperty = "TestValue"
            };

            var mockHttpClientContext = new MockHttpClientContext();
            mockHttpClientContext
                .Respond()
                .ReturnJson(expectedData);

            var client = mockHttpClientContext.CreateClient();

            var result = await client
                .GetAsync(new Uri("http://test.com"))
                .ReceiveJsonAsync(typeof(HttpTestDataObject));

            var data = (HttpTestDataObject) result;

            Assert.Single(mockHttpClientContext.RecordedRequests);
            Assert.Equal(expectedData.TestProperty, data.TestProperty);
        }

        [Fact]
        public async Task ReceiveJsonAsync_GetJsonObjectWith401_ThrowsException()
        {
            var expectedData = new HttpTestDataObject
            {
                TestProperty = "TestValue"
            };

            var mockHttpClientContext = new MockHttpClientContext();
            mockHttpClientContext
                .Respond()
                .ReturnJson(expectedData, HttpStatusCode.Unauthorized);

            var client = mockHttpClientContext.CreateClient();

            await Assert.ThrowsAsync<HttpRequestException>(() => client
                .GetAsync(new Uri("http://test.com"))
                .ReceiveJsonAsync(typeof(HttpTestDataObject)));
        }
    }
}