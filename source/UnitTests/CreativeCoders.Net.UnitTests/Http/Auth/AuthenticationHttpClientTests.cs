using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CreativeCoders.Net.Http;
using CreativeCoders.Net.Http.Auth;
using CreativeCoders.UnitTests.Net.Http;
using FakeItEasy;
using Xunit;

namespace CreativeCoders.Net.UnitTests.Http.Auth
{
    public class AuthenticationHttpClientTests
    {
        [Fact]
        public async Task SendAsync_FirstReturns401ThenData_ResponseContainsData()
        {
            const string expectedData = "TestData";

            var mockHttpClientContext = new MockHttpClientContext();
            mockHttpClientContext
                .Respond()
                .ReturnText("Failed", HttpStatusCode.Unauthorized)
                .Then()
                .ReturnText(expectedData, HttpStatusCode.OK);
            
            var httpMessageHandler = mockHttpClientContext.CreateMessageHandler();

            var httpMessageHandlerFactory = A.Fake<IHttpMessageHandlerFactory>();
            var clientAuthenticator = A.Fake<IHttpClientAuthenticator>();

            A.CallTo(() => clientAuthenticator.CanAuthenticate(A<Uri>.Ignored)).Returns(true);

            A.CallTo(() => httpMessageHandlerFactory.CreateHandler(A<string>.Ignored))
                .Returns(httpMessageHandler);

            var authHttpClient = new AuthenticationHttpClient(httpMessageHandlerFactory)
            {
                ClientAuthenticator = clientAuthenticator
            };

            var request = new HttpRequestMessage(HttpMethod.Get, new Uri("http://test.com"));

            var response = await authHttpClient
                .SendAsync(request)
                .ReceiveTextAsync();

            Assert.Equal(expectedData, response);

            A.CallTo(() => clientAuthenticator.PrepareHttpRequest(request)).MustHaveHappenedOnceExactly();
            A.CallTo(() => clientAuthenticator.CanAuthenticate(new Uri("http://test.com"))).MustHaveHappenedOnceExactly();
            A.CallTo(() => clientAuthenticator.AuthenticateAsync(new Uri("http://test.com")))
                .MustHaveHappenedOnceExactly();
        }
    }
}