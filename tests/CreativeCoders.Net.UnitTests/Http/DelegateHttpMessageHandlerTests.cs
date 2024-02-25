using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CreativeCoders.Net.Http;
using FluentAssertions;
using Microsoft.Win32.SafeHandles;
using Xunit;

namespace CreativeCoders.Net.UnitTests.Http;

public class DelegateHttpMessageHandlerTests
{
    [Fact]
    public async Task SendAsync_WithCancellationToken_RequestResponseCorrect()
    {
        // Arrange
        var expectedRequestMessage = new HttpRequestMessage(HttpMethod.Get, "http://test.com");

        var expectedResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);

        var expectedCancellationToken = new CancellationToken();

        HttpRequestMessage actualRequestMessage = null;

        var httpMessageHandler = new DelegateHttpMessageHandler((request, token) =>
        {
            actualRequestMessage = request;
            return Task.FromResult(expectedResponseMessage);
        });

        var httpClient = new HttpClient(httpMessageHandler);

        //  Act
        var response = await httpClient.SendAsync(expectedRequestMessage, expectedCancellationToken);

        // Assert
        actualRequestMessage
            .Should()
            .BeSameAs(expectedRequestMessage);

        response
            .Should()
            .BeSameAs(expectedResponseMessage);
    }

    [Fact]
    public async Task SendAsync_WithOutCancellationToken_RequestResponseCorrect()
    {
        // Arrange
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

        // Act
        var response = await httpClient.SendAsync(expectedRequestMessage, expectedCancellationToken);

        // Assert
        actualRequestMessage
            .Should()
            .BeSameAs(expectedRequestMessage);

        response
            .Should()
            .BeSameAs(expectedResponseMessage);
    }
}
