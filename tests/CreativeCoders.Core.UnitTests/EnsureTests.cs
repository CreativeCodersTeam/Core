using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using CreativeCoders.UnitTests;
using FluentAssertions;
using Xunit;

#nullable enable
namespace CreativeCoders.Core.UnitTests;

[Collection("FileSys")]
public class EnsureTests
{
    [Fact]
    public void IsNotNull_NoParamNameGiven_ParamNameIsSetByCompiler()
    {
        object? objectInstance = null;

        var act = () => Ensure.IsNotNull(objectInstance);

        act
            .Should()
            .Throw<ArgumentNullException>()
            .And.ParamName
            .Should()
            .Be(nameof(objectInstance));
    }

    [Fact]
    public void NotNull_NoParamNameGiven_ParamNameIsSetByCompiler()
    {
        object? objectInstance = null;

        var act = () => Ensure.NotNull(objectInstance);

        act
            .Should()
            .Throw<ArgumentNullException>()
            .And.ParamName
            .Should()
            .Be(nameof(objectInstance));
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void IsNotNullOrEmpty_NoParamNameGiven_ParamNameIsSetByCompiler(string? text)
    {
        var act = () => Ensure.IsNotNullOrEmpty(text);

        act
            .Should()
            .Throw<ArgumentException>()
            .And.ParamName
            .Should()
            .Be(nameof(text));
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    public void IsNotNullOrWhitespace_NoParamNameGiven_ParamNameIsSetByCompiler(string? text)
    {
        var act = () => Ensure.IsNotNullOrWhitespace(text);

        act
            .Should()
            .Throw<ArgumentException>()
            .And.ParamName
            .Should()
            .Be(nameof(text));
    }

    [Theory]
    [InlineData(new int[0])]
    [InlineData(null)]
    public void IsNotNullOrEmpty_ForEnumerableNoParamNameGiven_ParamNameIsSetByCompiler(
        IEnumerable<int>? items)
    {
        var act = () => Ensure.IsNotNullOrEmpty(items);

        act
            .Should()
            .Throw<ArgumentException>()
            .And.ParamName
            .Should()
            .Be(nameof(items));
    }

    [Fact]
    public void IsNotNull_AssertTestIsNotNull()
    {
        var instance = new object();

        object? nullObject = null;

        Assert.Throws<ArgumentNullException>(() => Ensure.IsNotNull(nullObject));
        Ensure.IsNotNull(instance);
        var obj = Ensure.NotNull(instance);

        obj
            .Should()
            .BeSameAs(instance);
    }

    [Fact]
    public void IsNotNullOrEmpty_AssertTestIsNotNullOrEmpty()
    {
        Assert.Throws<ArgumentException>(() => Ensure.IsNotNullOrEmpty(null));
        Assert.Throws<ArgumentException>(() => Ensure.IsNotNullOrEmpty(string.Empty));
        var value = Ensure.IsNotNullOrEmpty("test");

        value
            .Should()
            .Be("test");
    }

    [Fact]
    public void IsNotNullOrWhitespace_AssertTestIsNotNullOrWhitespace()
    {
        Assert.Throws<ArgumentException>(() => Ensure.IsNotNullOrWhitespace(null));
        Assert.Throws<ArgumentException>(() => Ensure.IsNotNullOrWhitespace(string.Empty));
        Assert.Throws<ArgumentException>(() => Ensure.IsNotNullOrWhitespace(" "));

        var value = Ensure.IsNotNullOrWhitespace("test");

        value
            .Should()
            .Be("test");
    }

    [Fact]
    public void GuidIsNotEmpty_AssertTestGuidIsNotEmpty()
    {
        Assert.Throws<ArgumentException>(() => Ensure.GuidIsNotEmpty(Guid.Empty));
        Ensure.GuidIsNotEmpty(Guid.NewGuid());
    }

    [Fact]
    public void That_AssertTestThat()
    {
        Assert.Throws<ArgumentException>(() => Ensure.That(false));
        Assert.Throws<ArgumentException>(() => Ensure.That(false, message: "param"));
        Ensure.That(true);
        Ensure.That(true, message: "param");
    }

    [Fact]
    public void IsNotNull_AssertIsNotNullExceptionTest()
    {
        object? obj = null;
        Assert.Throws<InvalidOperationException>(() =>
            Ensure.IsNotNull(obj, () => new InvalidOperationException("test")));

        obj = new object();
        Ensure.IsNotNull(obj, () => new InvalidOperationException("test"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void IsNotNullOrEmpty_AssertIsNotNullOrEmptyExceptionTestException(string? value)
    {
        var enumerable = value as IEnumerable<char>;
        Assert.Throws<ArgumentException>(() => Ensure.IsNotNullOrEmpty(enumerable));
    }

    [Theory]
    [InlineData("hello")]
    [InlineData(" world ")]
    public void IsNotNullOrEmpty_AssertIsNotNullOrEmptyExceptionTest(string value)
    {
        var enumerable = value as IEnumerable<char>;

        // Act
        var act = () => Ensure.IsNotNullOrEmpty(enumerable);

        // Assert
        act
            .Should()
            .NotThrow();
    }

    [Fact]
    public void ThatRange_AssertionTrue_PassWithoutException()
    {
        // Act
        var act0 = () => Ensure.ThatRange(true);
        var act1 = () => Ensure.ThatRange(true, message: "Message");

        // Assert
        act0
            .Should()
            .NotThrow();

        act1
            .Should()
            .NotThrow();
    }

    [Fact]
    public void ThatRange_AssertionFalse_ThrowsException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Ensure.ThatRange(false));
        Assert.Throws<ArgumentOutOfRangeException>(() => Ensure.ThatRange(false, message: "Message"));
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(0, 0, 1)]
    [InlineData(2, 1, 5)]
    [InlineData(-1, -2, 0)]
    public void IndexIsInRange_InRange_PassWithoutException(int index, int startIndex, int endIndex)
    {
        // Act
        var act = () => Ensure.IndexIsInRange(index, startIndex, endIndex);

        // Assert
        act
            .Should()
            .NotThrow();
    }

    [Theory]
    [InlineData(0, 1, 2)]
    [InlineData(0, 0, -1)]
    [InlineData(6, 1, 5)]
    public void IndexIsInRange_NotInRange_ThrowsException(int index, int startIndex, int endIndex)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            Ensure.IndexIsInRange(index, startIndex, endIndex));
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 2)]
    [InlineData(2, 5)]
    public void IndexIsInRange_InRangeOfCollectionLength_PassWithoutException(int index, int collectionLength)
    {
        // Act
        var act = () => Ensure.IndexIsInRange(index, collectionLength);

        // Assert
        act
            .Should()
            .NotThrow();
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(-1, 1)]
    [InlineData(3, 3)]
    public void IndexIsInRange_NotInRangeOfCollectionLength_ThrowsException(int index, int collectionLength)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            Ensure.IndexIsInRange(index, collectionLength));
    }

    [Fact]
    public void FileExists_ExistingFile_PassWithoutException()
    {
        var fileName = Path.GetTempFileName();

        var mockFileSystem = new MockFileSystemEx(
            new Dictionary<string, MockFileData>
            {
                { fileName, new MockFileData(string.Empty) }
            },
            Path.GetTempPath());

        mockFileSystem.Install();

        // Act
        var act = () => Ensure.FileExists(fileName);

        // Assert
        act
            .Should()
            .NotThrow();
    }

    [Fact]
    public void FileExists_NoneExistingFile_ThrowsException()
    {
        var fileName = Path.GetTempFileName();

        var mockFileSystem = new MockFileSystemEx();

        mockFileSystem.Install();

        Assert.Throws<FileNotFoundException>(() => Ensure.FileExists(fileName));
    }

    [Fact]
    public void DirectoryExists_ExistingDirectory_PassWithoutException()
    {
        var dirName = Path.GetTempPath();
        var fileName = Path.Combine(dirName, "test.txt");

        var mockFileSystem = new MockFileSystemEx(
            new Dictionary<string, MockFileData>
            {
                { fileName, new MockFileData(string.Empty) }
            },
            Path.GetTempPath());

        mockFileSystem.Install();

        // Act
        var act = () => Ensure.DirectoryExists(dirName);

        // Assert
        act
            .Should()
            .NotThrow();
    }

    [Fact]
    public void DirectoryExists_NoneExistingDirectory_ThrowsException()
    {
        var fileName = Path.GetTempFileName();

        var mockFileSystem = new MockFileSystemEx();

        mockFileSystem.Install();

        Assert.Throws<DirectoryNotFoundException>(() =>
            Ensure.DirectoryExists(Path.GetDirectoryName(fileName)));
    }

    [Theory]
    [InlineData("Test")]
    [InlineData("")]
    [InlineData(null)]
    public void Argument_DifferentValues_ValueAndNameAndHasValueAreCorrect(string? textValue)
    {
        // Act
        var argument = Ensure.Argument(textValue);

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
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    [SuppressMessage("SonarLint", "S3236")]
    public void Argument_ParamNameIsNull_ThrowsException()
    {
        // Act
        Action act = () => Ensure.Argument("Test", null!);

        // Assert
        act
            .Should()
            .Throw<ArgumentNullException>();
    }
}
