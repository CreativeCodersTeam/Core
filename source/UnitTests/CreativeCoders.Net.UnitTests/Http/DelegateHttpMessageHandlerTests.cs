using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CreativeCoders.Net.Http;
using Microsoft.Win32.SafeHandles;
using Xunit;

namespace CreativeCoders.Net.UnitTests.Http
{
    public class DelegateHttpMessageHandlerTests
    {
        [Fact]
        public async Task SendAsync_WithCancellationToken_RequestResponseAndCancellationTokenCorrect()
        {
            var expectedRequestMessage = new HttpRequestMessage(HttpMethod.Get, "http://test.com");

            var expectedResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);

            var expectedCancellationToken = new CancellationToken();

            HttpRequestMessage actualRequestMessage = null;
            SafeWaitHandle actualCancellationTokenWaitHandle = null;

            var httpMessageHandler = new DelegateHttpMessageHandler((request, token) =>
            {
                actualRequestMessage = request;
                actualCancellationTokenWaitHandle = token.WaitHandle.SafeWaitHandle;
                return Task.FromResult(expectedResponseMessage);
            });

            var httpClient = new HttpClient(httpMessageHandler);

            var response = await httpClient.SendAsync(expectedRequestMessage, expectedCancellationToken);

            Assert.Same(expectedRequestMessage, actualRequestMessage);
            Assert.True(expectedCancellationToken.WaitHandle.SafeWaitHandle.DangerousGetHandle()
                .Equals(actualCancellationTokenWaitHandle.DangerousGetHandle()));
            Assert.Same(expectedResponseMessage, response);
        }

        [Fact]
        public async Task SendAsync_WithOutCancellationToken_RequestResponseAndCancellationTokenCorrect()
        {
            var expectedRequestMessage = new HttpRequestMessage(HttpMethod.Get, "http://test.com");

            var expectedResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);

            var expectedCancellationToken = new CancellationToken();

            HttpRequestMessage actualRequestMessage = null;
            
            var httpMessageHandler = new DelegateHttpMessageHandler(request =>
            {
                actualRequestMessage = request;
                return Task.FromResult(expectedResponseMessage);
            });

            var httpClient = new HttpClient(httpMessageHandler);

            var response = await httpClient.SendAsync(expectedRequestMessage, expectedCancellationToken);

            Assert.Same(expectedRequestMessage, actualRequestMessage);
            Assert.Same(expectedResponseMessage, response);
        }
    }
}