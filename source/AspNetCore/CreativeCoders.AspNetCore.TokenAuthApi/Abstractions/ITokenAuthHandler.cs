using CreativeCoders.AspNetCore.TokenAuthApi.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CreativeCoders.AspNetCore.TokenAuthApi.Abstractions;

public interface ITokenAuthHandler
{
    /// <summary>
    /// Authenticates the user with the provided login credentials asynchronously.
    /// </summary>
    /// <param name="loginRequest">The login request details containing the user's credentials.</param>
    /// <param name="httpResponse">The HTTP response to write the authentication result to.</param>
    /// <returns>A task representing the asynchronous login operation.
    /// The task result is an IActionResult representing the result of the authentication process.</returns>
    Task<IActionResult> LoginAsync(LoginRequest loginRequest, HttpResponse httpResponse);

    Task<IActionResult> RefreshTokenAsync();

    Task<IActionResult> LogoutAsync();
}
