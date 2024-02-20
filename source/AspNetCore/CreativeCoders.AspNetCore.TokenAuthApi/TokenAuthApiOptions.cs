using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace CreativeCoders.AspNetCore.TokenAuthApi;

/// <summary>
///     Represents the options for configuring token authentication API.
/// </summary>
[PublicAPI]
[ExcludeFromCodeCoverage]
public class TokenAuthApiOptions
{
    /// <summary>
    ///     Gets or sets the domain for which the cookie is valid.
    /// </summary>
    /// <remarks>
    ///     The CookieDomain property represents the domain for which the cookie is valid.
    ///     The domain can be specified as a fully qualified domain name (e.g. "example.com")
    ///     or as a sub-domain (e.g. "subdomain.example.com").
    ///     If the CookieDomain property is not set, the cookie is valid for the current domain.
    ///     Setting the CookieDomain property to null or an empty string will cause the cookie
    ///     to be valid for the current domain only.
    /// </remarks>
    /// <value>
    ///     A string representing the domain for which the cookie is valid.
    ///     If not set, the cookie is valid for the current domain.
    /// </value>
    public string? CookieDomain { get; set; }

    /// <summary>
    ///     Gets or sets the path for the cookie.
    /// </summary>
    /// <remarks>
    ///     The path determines the scope or visibility of the cookie. Only requests within the specified path will include the
    ///     cookie.
    /// </remarks>
    public string CookiePath { get; set; } = "/";

    /// <summary>
    ///     The name of the authentication token.
    /// </summary>
    /// <value>
    ///     A string representing the name of the authentication token.
    ///     The default value is "auth_token".
    /// </value>
    public string AuthTokenName { get; set; } = "cc_auth_token";

    /// <summary>
    ///     The issuer of a auth token.
    /// </summary>
    /// <value>
    ///     The issuer represents the entity or authority that issues the token.
    /// </value>
    public string Issuer { get; set; } = "cc-token-auth-api";

    /// <summary>
    ///     Gets or sets a boolean value indicating whether to use refresh tokens for authentication.
    /// </summary>
    /// <value>
    ///     <c>true</c> if refresh tokens are used, otherwise <c>false</c>.
    /// </value>
    public bool UseRefreshTokens { get; set; }

    /// <summary>
    ///     Gets or sets the name of the refresh token property.
    /// </summary>
    public string RefreshTokenName { get; set; } = "cc_refresh_auth_token";

    /// <summary>
    ///     Gets or sets the domain for which the refresh cookie is valid.
    /// </summary>
    /// <remarks>
    ///     The CookieDomain property represents the domain for which the refresh cookie is valid.
    ///     The domain can be specified as a fully qualified domain name (e.g. "example.com")
    ///     or as a sub-domain (e.g. "subdomain.example.com").
    ///     If the RefreshCookieDomain property is not set, the cookie is valid for the current domain.
    ///     Setting the CookieDomain property to null or an empty string will cause the cookie
    ///     to be valid for the current domain only.
    /// </remarks>
    /// <value>
    ///     A string representing the domain for which the refresh cookie is valid.
    ///     If not set, the refresh cookie is valid for the current domain.
    /// </value>
    public string? RefreshCookieDomain { get; set; }

    /// <summary>
    ///     Gets or sets the path for the refresh cookie.
    /// </summary>
    /// <remarks>
    ///     The path determines the scope or visibility of the refresh cookie. Only requests within the specified path will
    ///     include the cookie.
    /// </remarks>
    public string RefreshCookiePath { get; set; } = "/";
}
