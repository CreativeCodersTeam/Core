using AwesomeAssertions;
using CreativeCoders.Cli.Core;
using Xunit;

namespace CreativeCoders.Cli.Tests.Core;

public class CommandResultTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(-1)]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    public void ImplicitOperatorTest(int exitCode)
    {
        // Act
        var result = (CommandResult)exitCode;

        // Assert
        result.ExitCode
            .Should().Be(exitCode);
    }
}
