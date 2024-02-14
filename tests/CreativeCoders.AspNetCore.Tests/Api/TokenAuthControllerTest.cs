using System.Diagnostics.CodeAnalysis;
using CreativeCoders.AspNetCore.TokenAuthApi.Api;
using CreativeCoders.AspNetCore.TokenAuthApi.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using JetBrains.Annotations;

namespace CreativeCoders.AspNetCore.Tests.Api
{
    [TestSubject(typeof(TokenAuthController))]
    public class TokenAuthControllerTest
    {
        private readonly ITokenAuthHandler _fakeTokenAuthHandler;

        private readonly TokenAuthController _tokenAuthController;

        public TokenAuthControllerTest()
        {
            _fakeTokenAuthHandler = A.Fake<ITokenAuthHandler>();
            _tokenAuthController = new TokenAuthController(_fakeTokenAuthHandler);
        }

        [Fact]
        public async Task LoginAsync_EmptyCredentials_ReturnsBadRequest()
        {
            // Arrange
            var loginRequest = new LoginRequest();

            var expectedResult = new OkResult();

            A.CallTo(() => _fakeTokenAuthHandler.LoginAsync(loginRequest, A<HttpResponse>.Ignored))
                .Returns(expectedResult);

            // Act
            var result = await _tokenAuthController.LoginAsync(loginRequest);

            // Assert
            result
                .Should()
                .BeOfType<UnauthorizedObjectResult>();

            A.CallTo(() => _fakeTokenAuthHandler.LoginAsync(loginRequest, A<HttpResponse>.Ignored))
                .MustNotHaveHappened();
        }

        [Fact]
        [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
        public async Task LoginAsync_Should_Return_Exception_When_LoginRequest_Is_Null()
        {
            // Act
            await Assert.ThrowsAsync<ArgumentNullException>(() => _tokenAuthController.LoginAsync(null!));
        }

        [Fact]
        public async Task LoginAsync_Should_Return_The_Same_HttpResult_As_TokenAuthHandler_LoginAsync()
        {
            // Arrange
            var loginRequest = new LoginRequest
            {
                UserName = "user1",
                Password = "pass1"
            };
            var httpResponse = new OkResult();

            A.CallTo(() => _fakeTokenAuthHandler.LoginAsync(loginRequest, A<HttpResponse>.Ignored))
                .Returns(httpResponse);

            // Act
            var result = await _tokenAuthController.LoginAsync(loginRequest);

            // Assert
            result
                .Should()
                .BeSameAs(httpResponse);

            A.CallTo(() => _fakeTokenAuthHandler.LoginAsync(loginRequest, A<HttpResponse>.Ignored))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task RefreshTokenAsync_Should_Invoke_TokenAuthHandler_RefreshTokenAsync()
        {
            // Arrange
            var refreshTokenRequest = new RefreshTokenRequest();

            A.CallTo(() =>
                    _fakeTokenAuthHandler.RefreshTokenAsync(refreshTokenRequest, A<HttpRequest>.Ignored,
                        A<HttpResponse>.Ignored))
                .Returns(new OkResult());

            // Act
            var result = await _tokenAuthController.RefreshTokenAsync(refreshTokenRequest);

            // Assert
            result
                .Should()
                .BeOfType<OkResult>();

            A.CallTo(() =>
                    _fakeTokenAuthHandler.RefreshTokenAsync(refreshTokenRequest, A<HttpRequest>.Ignored,
                        A<HttpResponse>.Ignored))
                .MustHaveHappenedOnceExactly();
        }
    }
}
