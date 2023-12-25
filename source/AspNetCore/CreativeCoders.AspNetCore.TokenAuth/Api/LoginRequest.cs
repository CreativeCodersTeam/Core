﻿using JetBrains.Annotations;

namespace CreativeCoders.AspNetCore.TokenAuth.Api;

[PublicAPI]
public class LoginRequest
{
    public string? UserName { get; set; }

    public string? Password { get; set; }

    public string? Domain { get; set; }
}