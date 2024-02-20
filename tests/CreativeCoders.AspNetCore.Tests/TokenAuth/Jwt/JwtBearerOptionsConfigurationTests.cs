using CreativeCoders.AspNetCore.TokenAuth.Jwt;

namespace CreativeCoders.AspNetCore.Tests.TokenAuth.Jwt;

public class JwtBearerOptionsConfigurationTests
{
    [Fact]
    public void Ctor_SecurityKeyIsNull_ThrowsException()
    {
        // Arrange
        var options = Options.Create(new JwtAuthenticationOptions());

        // Act
        Action act = () => _ = new JwtBearerOptionsConfiguration(options);

        // Assert
        act
            .Should()
            .Throw<InvalidOperationException>();
    }
}
