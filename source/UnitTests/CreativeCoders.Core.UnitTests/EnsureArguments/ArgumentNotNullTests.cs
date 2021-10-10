using System;
using FluentAssertions;
using Xunit;

#nullable enable
namespace CreativeCoders.Core.UnitTests.EnsureArguments
{
    public class ArgumentNotNullTests
    {
        [Theory]
        [InlineData("Test")]
        [InlineData("")]
        public void ArgumentNotNull_DifferentValues_ValueAndNameAndHasValueAreCorrect(string? textValue)
        {
            // Act
            var argument = Ensure.Argument(textValue, nameof(textValue)).NotNull();

            // Assert
            argument.Name
                .Should()
                .Be(nameof(textValue));

            argument.Value
                .Should()
                .Be(textValue);

            argument.HasValue()
                .Should()
                .Be(textValue != null);
        }

        [Fact]
        public void ArgumentNotNull_ParamNameIsNull_ThrowsException()
        {
            // Act
            Action act = () => Ensure.Argument("Test", null).NotNull();

            // Assert
            act
                .Should()
                .Throw<ArgumentNullException>();
        }
    }
}
