using AwesomeAssertions;
using CreativeCoders.Cli.Core;
using Xunit;

namespace CreativeCoders.Cli.Tests.Core;

public class OptionsValidationResultTests
{
    [Fact]
    public void StaticValid_Call_ReturnsValidResult()
    {
        // Act
        var result = OptionsValidationResult.Valid();

        // Assert
        result.IsValid
            .Should().BeTrue();

        result.Messages
            .Should().BeEmpty();
    }

    [Fact]
    public void StaticInvalid_Call_ReturnsInvalidResultWithMessages()
    {
        // Arrange
        const string message0 = "Test";
        const string message1 = "qwertz2";

        var messages = new[] { message0, message1 };

        // Act
        var result = OptionsValidationResult.Invalid(messages);

        // Assert
        result.IsValid
            .Should().BeFalse();

        result.Messages
            .Should().BeEquivalentTo(message0, message1);
    }

    [Fact]
    public void StaticInvalid_CallWithMessagesNull_ReturnsInvalidResultWithEmptyMessages()
    {
        // Act
        var result = OptionsValidationResult.Invalid(null);

        // Assert
        result.IsValid
            .Should().BeFalse();

        result.Messages
            .Should().BeEmpty();
    }
}
