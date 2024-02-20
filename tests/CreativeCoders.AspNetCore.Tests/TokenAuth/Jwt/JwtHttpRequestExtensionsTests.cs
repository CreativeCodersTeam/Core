using CreativeCoders.AspNetCore.TokenAuth.Jwt;

namespace CreativeCoders.AspNetCore.Tests.TokenAuth.Jwt;

public class JwtHttpRequestExtensionsTests
{
    [Fact]
    public void GetJwtToken_OneValidBearerTokenIsInAuthHeader_ReturnsTokenFromAuthHeader()
    {
        // Arrange
        const string expectedToken = "expectedToken";

        var httpRequest = A.Fake<HttpRequest>();

        A.CallTo(() => httpRequest.Headers.Authorization)
            .Returns(new[] { $"Bearer {expectedToken}" });

        // Act
        var actualToken = httpRequest.GetJwtToken("");

        // Assert
        actualToken
            .Should()
            .Be(expectedToken);
    }

    [Fact]
    public void GetJwtToken_MultipleTokensInAuthHeader_ReturnsBearerTokenFromAuthHeader()
    {
        // Arrange
        const string expectedToken = "expectedToken";

        var httpRequest = A.Fake<HttpRequest>();

        A.CallTo(() => httpRequest.Headers.Authorization)
            .Returns(new[] { "Token test", $"Bearer {expectedToken}" });

        // Act
        var actualToken = httpRequest.GetJwtToken("");

        // Assert
        actualToken
            .Should()
            .Be(expectedToken);
    }

    [Fact]
    public void GetJwtToken_NoTokenIsInAuthHeader_ReturnsEmptyString()
    {
        // Arrange
        var httpRequest = A.Fake<HttpRequest>();

        // Act
        var actualToken = httpRequest.GetJwtToken("");

        // Assert
        actualToken
            .Should()
            .Be(string.Empty);
    }

    [Fact]
    public void GetJwtToken_NoTokenIsInAuthHeaderButCookie_ReturnsTokenFromCookie()
    {
        // Arrange
        const string expectedToken = "expectedToken";

        var httpRequest = A.Fake<HttpRequest>();

        A.CallTo(() => httpRequest.Cookies["auth_cookie"])
            .Returns(expectedToken);

        // Act
        var actualToken = httpRequest.GetJwtToken("auth_cookie");

        // Assert
        actualToken
            .Should()
            .Be(expectedToken);
    }

    [Fact]
    public void GetJwtToken_OtherTokenIsInAuthHeaderButAlsoCookie_ReturnsTokenFromCookie()
    {
        // Arrange
        const string expectedToken = "expectedToken";

        var httpRequest = A.Fake<HttpRequest>();

        A.CallTo(() => httpRequest.Headers.Authorization)
            .Returns(new[] { "OtherBearer 12345" });

        A.CallTo(() => httpRequest.Cookies["auth_cookie"])
            .Returns(expectedToken);

        // Act
        var actualToken = httpRequest.GetJwtToken("auth_cookie");

        // Assert
        actualToken
            .Should()
            .Be(expectedToken);
    }
}
