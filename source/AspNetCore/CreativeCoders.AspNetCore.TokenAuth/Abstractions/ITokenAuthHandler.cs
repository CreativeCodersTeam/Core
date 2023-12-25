using System.Threading.Tasks;
using CreativeCoders.AspNetCore.TokenAuth.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CreativeCoders.AspNetCore.TokenAuth.Abstractions;

public interface ITokenAuthHandler
{
    Task<IActionResult> LoginAsync(LoginRequest loginRequest, HttpResponse httpResponse);

    Task<IActionResult> RefreshTokenAsync();

    Task<IActionResult> LogoutAsync();
}
