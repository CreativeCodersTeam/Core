using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CreativeCoders.Net.Http.Auth;
using CreativeCoders.UnitTests.Net.Http;
using FakeItEasy;
using Xunit;

namespace CreativeCoders.Net.UnitTests.Http.Auth
{
    public class AuthenticationHttpMessageHandlerTests
    {
        [Fact]
        public async Task SetAuthenticationProvider_NoAuthenticationProvider401_NoRetryAndThrowsException()
        {
            const string expectedData = "TestData";

            var mockHttpClientContext = new MockHttpClientContext();
            mockHttpClientContext
                .Respond()
                .ReturnText("Failed", HttpStatusCode.Unauthorized)
                .Then()
                .ReturnText(expectedData, HttpStatusCode.OK);

            var authenticationHttpMessageHandler = new AuthenticationHttpMessageHandler(mockHttpClientContext.CreateMessageHandler());

            var httpClient = new HttpClient(authenticationHttpMessageHandler);

            await Assert.ThrowsAsync<HttpRequestException>(() => httpClient.GetStringAsync(new Uri("http://test.com")));

            Assert.Single(mockHttpClientContext.RecordedRequests);
        }

        [Fact]
        public async Task SetAuthenticationProvider_NoAuthenticationProvider200_ReturnsData()
        {
            const string expectedData = "TestData";

            var mockHttpClientContext = new MockHttpClientContext();
            mockHttpClientContext
                .Respond()
                .ReturnText(expectedData, HttpStatusCode.OK);

            var authenticationHttpMessageHandler = new AuthenticationHttpMessageHandler(mockHttpClientContext.CreateMessageHandler());

            var httpClient = new HttpClient(authenticationHttpMessageHandler);

            var response = await httpClient.GetStringAsync(new Uri("http://test.com"));

            Assert.Equal(expectedData, response);
            Assert.Single(mockHttpClientContext.RecordedRequests);
        }

        [Fact]
        public async Task SetAuthenticationProvider_AuthenticationProvider401_DoRetry()
        {
            const string expectedData = "TestData";
            const string expectedScheme = "AuthScheme";
            const string expectedBearer = "1234567890";

            var mockHttpClientContext = new MockHttpClientContext();
            mockHttpClientContext
                .Respond()
                .ReturnText("Failed", HttpStatusCode.Unauthorized)
                .Then()
                .ReturnText(expectedData, HttpStatusCode.OK);

            var clientAuthenticator = A.Fake<IHttpClientAuthenticator>();
            A.CallTo(() => clientAuthenticator.CanAuthenticate(A<Uri>.Ignored)).Returns(true);
            A.CallTo(() => clientAuthenticator.AuthenticateAsync(new Uri("http://test.com")))
                .Returns(Task.CompletedTask);
            A.CallTo(() => clientAuthenticator.PrepareHttpRequest(A<HttpRequestMessage>.Ignored))
                .Invokes(x =>
                {
                    var requestArgument = x.Arguments.Get<HttpRequestMessage>(0);

                    if (requestArgument == null)
                    {
                        return;
                    }

                    requestArgument.Headers.Authorization =
                            new AuthenticationHeaderValue(expectedScheme, expectedBearer);
                });

            var authenticationProvider = A.Fake<IHttpClientAuthenticationProvider>();
            A.CallTo(() => authenticationProvider.ClientAuthenticator).Returns(clientAuthenticator);

            var authenticationHttpMessageHandler = new AuthenticationHttpMessageHandler(mockHttpClientContext.CreateMessageHandler());
            authenticationHttpMessageHandler.SetAuthenticationProvider(authenticationProvider);

            var httpClient = new HttpClient(authenticationHttpMessageHandler);

            var response = await httpClient.GetStringAsync(new Uri("http://test.com"));

            Assert.Equal(expectedData, response);

            Assert.Equal(2, mockHttpClientContext.RecordedRequests.Count);

            var request = mockHttpClientContext.RecordedRequests.Last();

            var authHeader = request.RequestMessage.Headers.Authorization;

            Assert.Equal(expectedScheme, authHeader.Scheme);
            Assert.Equal(expectedBearer, authHeader.Parameter);
        }

        [Fact]
        public async Task SetAuthenticationProvider_AuthenticationProviderSetToNullBeforeRetry_ThrowsException()
        {
            const string expectedData = "TestData";
            const string expectedScheme = "AuthScheme";
            const string expectedBearer = "1234567890";

            var mockHttpClientContext = new MockHttpClientContext();
            mockHttpClientContext
                .Respond()
                .ReturnText("Failed", HttpStatusCode.Unauthorized)
                .Then()
                .ReturnText(expectedData, HttpStatusCode.OK);

            var clientAuthenticator = A.Fake<IHttpClientAuthenticator>();
            A.CallTo(() => clientAuthenticator.CanAuthenticate(A<Uri>.Ignored)).Returns(true);
            A.CallTo(() => clientAuthenticator.AuthenticateAsync(new Uri("http://test.com")))
                .Returns(Task.CompletedTask);
            A.CallTo(() => clientAuthenticator.PrepareHttpRequest(A<HttpRequestMessage>.Ignored))
                .Invokes(x =>
                {
                    var requestArgument = x.Arguments.Get<HttpRequestMessage>(0);

                    if (requestArgument == null)
                    {
                        return;
                    }

                    requestArgument.Headers.Authorization =
                            new AuthenticationHeaderValue(expectedScheme, expectedBearer);
                });

            var authenticationProvider = A.Fake<IHttpClientAuthenticationProvider>();
            A.CallTo(() => authenticationProvider.ClientAuthenticator).Returns(clientAuthenticator).Once().Then.Returns(null);

            var authenticationHttpMessageHandler = new AuthenticationHttpMessageHandler(mockHttpClientContext.CreateMessageHandler());
            authenticationHttpMessageHandler.SetAuthenticationProvider(authenticationProvider);

            var httpClient = new HttpClient(authenticationHttpMessageHandler);

            await Assert.ThrowsAsync<ArgumentException>(() => httpClient.GetStringAsync(new Uri("http://test.com")));
        }
    }
}