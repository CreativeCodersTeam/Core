using System;
using System.Collections.Generic;
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
    public void IsNotNull_AssertTestIsNotNull()
    {
        var instance = new object();

        object nullObject = null!;

        Assert.Throws<ArgumentNullException>(() => Ensure.IsNotNull(nullObject, "param"));
        Ensure.IsNotNull(instance, nameof(instance));
        var obj = Ensure.NotNull(instance, nameof(instance));

        obj
            .Should()
            .BeSameAs(instance);
    }

    [Fact]
    public void IsNotNullOrEmpty_AssertTestIsNotNullOrEmpty()
    {
        Assert.Throws<ArgumentException>(() => Ensure.IsNotNullOrEmpty(null, "string"));
        Assert.Throws<ArgumentException>(() => Ensure.IsNotNullOrEmpty(string.Empty, "string"));
        var value = Ensure.IsNotNullOrEmpty("test", "string");

        value
            .Should()
            .Be("test");
    }

    [Fact]
    public void IsNotNullOrWhitespace_AssertTestIsNotNullOrWhitespace()
    {
        Assert.Throws<ArgumentException>(() => Ensure.IsNotNullOrWhitespace(null, "string"));
        Assert.Throws<ArgumentException>(() => Ensure.IsNotNullOrWhitespace(string.Empty, "string"));
        Assert.Throws<ArgumentException>(() => Ensure.IsNotNullOrWhitespace(" ", "string"));

        var value = Ensure.IsNotNullOrWhitespace("test", "string");

        value
            .Should()
            .Be("test");
    }

    [Fact]
    public void GuidIsNotEmpty_AssertTestGuidIsNotEmpty()
    {
        Assert.Throws<ArgumentException>(() => Ensure.GuidIsNotEmpty(Guid.Empty, "guid"));
        Ensure.GuidIsNotEmpty(Guid.NewGuid(), "guid");
    }

    [Fact]
    public void That_AssertTestThat()
    {
        Assert.Throws<ArgumentException>(() => Ensure.That(false, "param"));
        Assert.Throws<ArgumentException>(() => Ensure.That(false, "message", "param"));
        Ensure.That(true, "param");
        Ensure.That(true, "message", "param");
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
        Assert.Throws<ArgumentException>(() => Ensure.IsNotNullOrEmpty(enumerable, nameof(value)));
    }

    [Theory]
    [InlineData("hello")]
    [InlineData(" world ")]
    public void IsNotNullOrEmpty_AssertIsNotNullOrEmptyExceptionTest(string value)
    {
        var enumerable = value as IEnumerable<char>;
        Ensure.IsNotNullOrEmpty(enumerable, nameof(value));
    }

    [Fact]
    public void ThatRange_AssertionTrue_PassWithoutException()
    {
        Ensure.ThatRange(true, "paramName");
        Ensure.ThatRange(true, "paramName", "Message");
    }

    [Fact]
    public void ThatRange_AssertionFalse_ThrowsException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Ensure.ThatRange(false, "paramName"));
        Assert.Throws<ArgumentOutOfRangeException>(() => Ensure.ThatRange(false, "paramName", "Message"));
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(0, 0, 1)]
    [InlineData(2, 1, 5)]
    [InlineData(-1, -2, 0)]
    public void IndexIsInRange_InRange_PassWithoutException(int index, int startIndex, int endIndex)
    {
        Ensure.IndexIsInRange(index, startIndex, endIndex, "paramName");
    }

    [Theory]
    [InlineData(0, 1, 2)]
    [InlineData(0, 0, -1)]
    [InlineData(6, 1, 5)]
    public void IndexIsInRange_NotInRange_ThrowsException(int index, int startIndex, int endIndex)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            Ensure.IndexIsInRange(index, startIndex, endIndex, "paramName"));
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 2)]
    [InlineData(2, 5)]
    public void IndexIsInRange_InRangeOfCollectionLength_PassWithoutException(int index, int collectionLength)
    {
        Ensure.IndexIsInRange(index, collectionLength, "paramName");
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(-1, 1)]
    [InlineData(3, 3)]
    public void IndexIsInRange_NotInRangeOfCollectionLength_ThrowsException(int index, int collectionLength)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            Ensure.IndexIsInRange(index, collectionLength, "paramName"));
    }

    [Fact]
    public void FileExists_ExistingFile_PassWithoutException()
    {
        const string fileName = @"C:\temp_dir\test.txt";

        var mockFileSystem = new MockFileSystemEx(
            new Dictionary<string, MockFileData>
            {
                {fileName, MockFileData.NullObject}
            },
            @"C:\");

        mockFileSystem.Install();

        Ensure.FileExists(fileName);
    }

    [Fact]
    public void FileExists_NoneExistingFile_ThrowsException()
    {
        const string fileName = @"C:\temp_dir\test.txt";

        var mockFileSystem = new MockFileSystemEx();

        mockFileSystem.Install();

        Assert.Throws<FileNotFoundException>(() => Ensure.FileExists(fileName));
    }

    [Fact]
    public void DirectoryExists_ExistingDirectory_PassWithoutException()
    {
        const string dirName = @"C:\temp_dir";
        const string fileName = dirName + @"\test.txt";

        var mockFileSystem = new MockFileSystemEx(
            new Dictionary<string, MockFileData>
            {
                {fileName, MockFileData.NullObject}
            },
            @"C:\");

        mockFileSystem.Install();

        Ensure.DirectoryExists(dirName);
    }

    [Fact]
    public void DirectoryExists_NoneExistingDirectory_ThrowsException()
    {
        const string fileName = @"C:\temp_dir\test.txt";

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
        var argument = Ensure.Argument(textValue, nameof(textValue));

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
