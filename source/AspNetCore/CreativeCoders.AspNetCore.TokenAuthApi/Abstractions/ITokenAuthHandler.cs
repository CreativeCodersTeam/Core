using CreativeCoders.AspNetCore.TokenAuthApi.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CreativeCoders.AspNetCore.TokenAuthApi.Abstractions;

public interface ITokenAuthHandler
{
    /// <summary>
    ///     Authenticates the user with the provided login credentials asynchronously.
    /// </summary>
    /// <param name="loginRequest">The login request details containing the user's credentials.</param>
    /// <param name="httpResponse">The HTTP response to write the authentication result to.</param>
    /// <returns>
    ///     A task representing the asynchronous login operation.
    ///     The task result is an IActionResult representing the result of the authentication process.
    /// </returns>
    Task<IActionResult> LoginAsync(LoginRequest loginRequest, HttpResponse httpResponse);

    /// <summary>
    ///     Asynchronously performs logout operation.
    /// </summary>
    /// <param name="logoutRequest"></param>
    /// <param name="request"></param>
    /// <param name="response">
    ///     An instance of <see cref="Microsoft.AspNetCore.Http.HttpResponse" /> representing the HTTP response for the client.
    /// </param>
    /// <returns>
    ///     A <see cref="System.Threading.Tasks.Task" /> that represents the asynchronous logout operation.
    ///     The task result is an instance of <see cref="Microsoft.AspNetCore.Mvc.IActionResult" /> representing the result of
    ///     the logout process.
    /// </returns>
    Task<IActionResult> LogoutAsync(LogoutRequest logoutRequest, HttpRequest request, HttpResponse response);

    /// <summary>
    ///     Asynchronously refreshes an existing token.
    /// </summary>
    /// <param name="refreshTokenRequest">
    ///     An instance of <see cref="RefreshTokenRequest" /> containing the details of the token
    ///     to be refreshed.
    /// </param>
    /// <param name="httpRequest">The original HTTP request that triggered the token refresh.</param>
    /// <param name="httpResponse">The original HTTP response for that request</param>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation. The task result is an instance of
    ///     <see cref="IActionResult" />.
    ///     It represents the result of the refresh token process, typically contains data for a new valid token or an error if
    ///     the refresh process failed.
    /// </returns>
    Task<IActionResult> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest, HttpRequest httpRequest,
        HttpResponse httpResponse);
}
