using System;
using FluentAssertions;
using Xunit;

#nullable enable
namespace CreativeCoders.Core.UnitTests.EnsureArguments.Extensions
{
    public class MustArgumentExtensionsTests
    {
        [Fact]
        public void Must_WithValueMatching_ReturnsArgumentWithValue()
        {
            const string testValue = "Test";

            // Act
            var argument = Ensure.Argument(testValue, nameof(testValue))
                .Must(x => x?.StartsWith("Te") == true);

            // Assert
            argument.Value
                .Should()
                .Be(testValue);
        }

        [Fact]
        public void Must_WithValueNotMatching_ThrowsException()
        {
            const string testValue = "Test";

            // Act
            Action act = () => Ensure.Argument(testValue, nameof(testValue))
                .Must(x => x?.StartsWith("12") == true);

            // Assert
            act
                .Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        public void Must_WithValueNotMatchingAndMessage_ThrowsExceptionWithMessage()
        {
            const string testValue = "Test";
            const string message = "TestMessage";

            // Act
            Action act = () => Ensure.Argument(testValue, nameof(testValue))
                .Must(x => x?.StartsWith("12") == true, message);

            // Assert
            act
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(ExceptionHelper.GetMessage(message, nameof(testValue)));
        }

        [Fact]
        public void Must_ArgNotNullWithValueMatching_ReturnsArgumentWithValue()
        {
            const string testValue = "Test";

            // Act
            var argument = Ensure.Argument(testValue, nameof(testValue)).NotNull()
                .Must(x => x?.StartsWith("Te") == true);

            // Assert
            argument.Value
                .Should()
                .Be(testValue);
        }

        [Fact]
        public void Must_ArgNotNullWithValueNotMatching_ThrowsException()
        {
            const string testValue = "Test";

            // Act
            Action act = () => Ensure.Argument(testValue, nameof(testValue)).NotNull()
                .Must(x => x?.StartsWith("12") == true);

            // Assert
            act
                .Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        public void Must_ArgNotNullWithValueNotMatchingAndMessage_ThrowsExceptionWithMessage()
        {
            const string testValue = "Test";
            const string message = "TestMessage";

            // Act
            Action act = () => Ensure.Argument(testValue, nameof(testValue)).NotNull()
                .Must(x => x?.StartsWith("12") == true, message);

            // Assert
            act
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(ExceptionHelper.GetMessage(message, nameof(testValue)));
        }


        [Fact]
        public void MustNot_WithValueNotMatching_ReturnsArgumentWithValue()
        {
            const string testValue = "Test";

            // Act
            var argument = Ensure.Argument(testValue, nameof(testValue))
                .MustNot(x => x?.StartsWith("12") == true);

            // Assert
            argument.Value
                .Should()
                .Be(testValue);
        }

        [Fact]
        public void MustNot_WithValueMatching_ThrowsException()
        {
            const string testValue = "Test";

            // Act
            Action act = () => Ensure.Argument(testValue, nameof(testValue))
                .MustNot(x => x?.StartsWith("Te") == true);

            // Assert
            act
                .Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        public void MustNot_WithValueMatchingAndMessage_ThrowsExceptionWithMessage()
        {
            const string testValue = "Test";
            const string message = "TestMessage";

            // Act
            Action act = () => Ensure.Argument(testValue, nameof(testValue))
                .MustNot(x => x?.StartsWith("Te") == true, message);

            // Assert
            act
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(ExceptionHelper.GetMessage(message, nameof(testValue)));
        }

        [Fact]
        public void MustNot_ArgNotNullWithValueNotMatching_ReturnsArgumentWithValue()
        {
            const string testValue = "Test";

            // Act
            var argument = Ensure.Argument(testValue, nameof(testValue)).NotNull()
                .MustNot(x => x?.StartsWith("12") == true);

            // Assert
            argument.Value
                .Should()
                .Be(testValue);
        }

        [Fact]
        public void MustNot_ArgNotNullWithValueMatching_ThrowsException()
        {
            const string testValue = "Test";

            // Act
            Action act = () => Ensure.Argument(testValue, nameof(testValue)).NotNull()
                .MustNot(x => x?.StartsWith("Te") == true);

            // Assert
            act
                .Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        public void MustNot_ArgNotNullWithValueMatchingAndMessage_ThrowsExceptionWithMessage()
        {
            const string testValue = "Test";
            const string message = "TestMessage";

            // Act
            Action act = () => Ensure.Argument(testValue, nameof(testValue)).NotNull()
                .MustNot(x => x?.StartsWith("Te") == true, message);

            // Assert
            act
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(ExceptionHelper.GetMessage(message, nameof(testValue)));
        }
    }
}
