using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CreativeCoders.AspNetCore.TokenAuth;

public interface ITokenAuthHandler
{
    Task<IActionResult> LoginAsync(LoginRequest loginRequest, HttpResponse httpResponse);

    Task<IActionResult> RefreshTokenAsync();

    Task<IActionResult> LogoutAsync();
}
