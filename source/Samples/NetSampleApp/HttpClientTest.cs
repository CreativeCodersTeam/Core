using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CreativeCoders.Net.Http;
using CreativeCoders.UnitTests.Net.Http;

namespace NetSampleApp
{
    [SuppressMessage("ReSharper", "UnusedVariable")]
    public class HttpClientTest
    {
        public async Task Run()
        {
            var mockHttpClientContext = new MockHttpClientContext();

            mockHttpClientContext
                .Respond()
                .ForUri("http://test.com")
                .ReturnText("Index", HttpStatusCode.OK);

            mockHttpClientContext
                .Respond()
                .ForUri("http://test.com/item/1/")
                .ReturnText("Test1234", HttpStatusCode.OK);

            mockHttpClientContext
                .Respond()
                .ForUri("http://test.com/item/2/")
                .ReturnText("qwertz", HttpStatusCode.OK);

            mockHttpClientContext
                .Respond()
                .WithVerb(HttpMethod.Post)
                .ReturnText("PostData", HttpStatusCode.OK);

            var client = mockHttpClientContext.CreateClient();

            var index = await client.GetStringAsync("http://test.com");

            var item1 = await client.GetStringAsync("http://test.com/item/1/");

            var item2 = await client.GetStringAsync("http://test.com/item/2/");

            var postResult = await client
                .PostAsync(new Uri("http://test1.com"), new StringContent(""))
                .ReceiveTextAsync();
        }
    }
}