﻿using CreativeCoders.AspNetCore.TokenAuthApi.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CreativeCoders.AspNetCore.TokenAuthApi.Abstractions;

public interface ITokenAuthHandler
{
    Task<IActionResult> LoginAsync(LoginRequest loginRequest, HttpResponse httpResponse);

    Task<IActionResult> RefreshTokenAsync();

    Task<IActionResult> LogoutAsync();
}