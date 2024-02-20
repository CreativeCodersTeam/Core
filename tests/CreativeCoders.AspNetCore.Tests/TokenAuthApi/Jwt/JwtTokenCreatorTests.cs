using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CreativeCoders.AspNetCore.TokenAuthApi.Jwt;
using JetBrains.Annotations;
using Microsoft.IdentityModel.Tokens;

namespace CreativeCoders.AspNetCore.Tests.TokenAuthApi.Jwt;

[TestSubject(typeof(JwtTokenCreator))]
public class JwtTokenCreatorTests
{
    [Fact]
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public void JwtTokenCreator_GivenNullOptions_ThrowsException()
    {
        // Arrange
        IOptions<JwtTokenAuthApiOptions>? nullOptions = null;

        // Act
        var act = () => { _ = new JwtTokenCreator(nullOptions!); };

        // Assert
        act
            .Should()
            .Throw<ArgumentNullException>();
    }

    [Fact]
    public void JwtTokenCreator_GivenOptionsWithoutSecurityKey_ThrowsException()
    {
        // Arrange
        var options = Options.Create(new JwtTokenAuthApiOptions { SecurityKey = null });

        // Act
        var act = () => { _ = new JwtTokenCreator(options); };

        // Assert
        act
            .Should()
            .Throw<InvalidOperationException>();
    }

    [Fact]
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public async Task CreateTokenAsync_GivenNullUserName_ThrowsException()
    {
        // Arrange
        var securityKey = new SymmetricSecurityKey(Enumerable.Range(0, 127).Select(x => (byte)x).ToArray());
        var options = Options.Create(new JwtTokenAuthApiOptions { SecurityKey = securityKey });

        var jwtTokenCreator = new JwtTokenCreator(options);

        // Act
        var act = async () =>
        {
            await jwtTokenCreator.CreateTokenAsync("issuer", null!, Array.Empty<Claim>());
        };

        // Assert
        await act
            .Should()
            .ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task CreateTokenAsync_GivenNotNullParameters_Successful()
    {
        // Arrange
        var securityKey =
            new SymmetricSecurityKey(Enumerable.Range(0, 127).Select(x => (byte)x).ToArray());
        var options = Options.Create(new JwtTokenAuthApiOptions { SecurityKey = securityKey });

        var jwtTokenCreator = new JwtTokenCreator(options);

        // Act
        var result = await jwtTokenCreator.CreateTokenAsync("issuer", "test",
            new List<Claim> { new Claim(ClaimTypes.Name, "test") });

        // Assert
        result
            .Should()
            .NotBeNull();

        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(result);

        token
            .Should()
            .NotBeNull();

        token.Claims
            .Should()
            .Contain(s => s.Type == ClaimTypes.Name && s.Value == "test");
    }

    [Fact]
    public async Task CreateTokenAsync_GivenNotNullParametersAndSecurityKeyHaveSpaces_Successful()
    {
        // Arrange
        var securityKey = new SymmetricSecurityKey(
            Encoding.ASCII.GetBytes("test key with spaces test key with spaces test key with spaces"));

        var options = Options.Create(new JwtTokenAuthApiOptions { SecurityKey = securityKey });

        var jwtTokenCreator = new JwtTokenCreator(options);

        // Act
        var result = await jwtTokenCreator.CreateTokenAsync("issuer", "test",
            new List<Claim> { new Claim(ClaimTypes.Name, "test") });

        // Assert
        result
            .Should()
            .NotBeNull();

        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(result);

        token
            .Should()
            .NotBeNull();

        token.Claims
            .Should()
            .Contain(s => s.Type == ClaimTypes.Name && s.Value == "test");

        token.ValidTo
            .Should()
            .BeCloseTo(DateTime.UtcNow.Add(options.Value.ExpirationTimeSpan), TimeSpan.FromSeconds(2));
    }

    [Fact]
    public async Task CreateTokenAsync_GivenCustomExpirationTimeSpan_ValidToIsCorrect()
    {
        // Arrange
        var securityKey = new SymmetricSecurityKey(
            Encoding.ASCII.GetBytes("test key with spaces test key with spaces test key with spaces"));

        var options = Options.Create(new JwtTokenAuthApiOptions
        {
            SecurityKey = securityKey,
            ExpirationTimeSpan = TimeSpan.FromHours(2)
        });

        var jwtTokenCreator = new JwtTokenCreator(options);

        // Act
        var result = await jwtTokenCreator.CreateTokenAsync("issuer", "test",
            new List<Claim> { new Claim(ClaimTypes.Name, "test") });

        // Assert
        result
            .Should()
            .NotBeNull();

        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(result);

        token
            .Should()
            .NotBeNull();

        token.Claims
            .Should()
            .Contain(s => s.Type == ClaimTypes.Name && s.Value == "test");

        token.ValidTo
            .Should()
            .BeCloseTo(DateTime.UtcNow.Add(options.Value.ExpirationTimeSpan), TimeSpan.FromSeconds(2));
    }

    [Fact]
    public async Task ReadTokenFromStringAsync_ValidJwtTokenIsGiven_TokenIsRead()
    {
        const string testIssuer = "issuer";
        const string testUser = "testUser";

        // Arrange
        var securityKey =
            new SymmetricSecurityKey(Enumerable.Range(0, 127).Select(x => (byte)x).ToArray());
        var options = Options.Create(new JwtTokenAuthApiOptions { SecurityKey = securityKey });

        var jwtTokenCreator = new JwtTokenCreator(options);

        var additionalClaims = new List<Claim> { new Claim(ClaimTypes.Name, testUser) };

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            testIssuer,
            "",
            additionalClaims,
            expires: DateTime.Now.Add(TimeSpan.FromHours(1)),
            signingCredentials: credentials);

        var createdToken = new JwtSecurityTokenHandler().WriteToken(token);

        // Act
        var readToken = await jwtTokenCreator.ReadTokenFromStringAsync(createdToken);

        // Assert
        readToken
            .Should()
            .NotBeNull();

        readToken.Claims
            .Should()
            .Contain(s => s.Type == ClaimTypes.Name && s.Value == testUser);
    }
}
