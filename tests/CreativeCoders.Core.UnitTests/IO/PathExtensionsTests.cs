using System.IO.Abstractions;
using CreativeCoders.Core.IO;
using FakeItEasy;
using AwesomeAssertions;
using Xunit;

namespace CreativeCoders.Core.UnitTests.IO;

public class PathExtensionsTests
{
    [Theory]
    [InlineData("file", new[] { 'a' }, "_", "file")]
    [InlineData("file", new[] { 'f' }, "_", "_ile")]
    [InlineData("file", new[] { 'f', 'i' }, "_", "__le")]
    [InlineData("file", new[] { 'f', 'i', 'l' }, "_", "___e")]
    [InlineData("file12", new[] { 'f', 'i', '1' }, "_", "__le_2")]
    public void ReplaceInvalidFileNameChars_WithFileNameAndReplacement_ReturnsFileNameWithReplacedChars(
        string fileName, char[] invalidChars, string replacement, string expected)
    {
        // Arrange
        var path = A.Fake<IPath>();

        A.CallTo(() => path.GetInvalidFileNameChars())
            .Returns(invalidChars);

        // Act
        var result = path.ReplaceInvalidFileNameChars(fileName, replacement);

        // Assert
        result
            .Should()
            .Be(expected);
    }
}
