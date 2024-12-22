using System.IO;
using System.IO.Abstractions;
using System.Threading;
using System.Threading.Tasks;
using CreativeCoders.Core.IO;
using CreativeCoders.Options.Core;
using CreativeCoders.Options.Storage.FileSystem;
using FakeItEasy;
using Microsoft.Extensions.Options;
using Xunit;

#nullable enable

namespace CreativeCoders.Core.UnitTests.Options.Storage.FileSystem;

[Collection("FileSys")]
public class FileSystemOptionsStorageProviderTests
{
    private readonly IFile _file;

    private readonly IOptionsMonitorCache<TestOptions> _optionsCache;

    private readonly IOptionsStorageDataSerializer<TestOptions> _optionsSerializer;

    private readonly IPath _path;

    private readonly FileSystemOptionsStorageProvider<TestOptions> _storageProvider;

    public FileSystemOptionsStorageProviderTests()
    {
        _optionsCache = A.Fake<IOptionsMonitorCache<TestOptions>>();
        _optionsSerializer = A.Fake<IOptionsStorageDataSerializer<TestOptions>>();
        _storageProvider =
            new FileSystemOptionsStorageProvider<TestOptions>(_optionsCache, _optionsSerializer);

        var fileSystem = A.Fake<IFileSystemEx>();
        FileSys.InstallFileSystemSupport(fileSystem);

        _file = A.Fake<IFile>();
        A.CallTo(() => fileSystem.File).Returns(_file);

        _path = A.Fake<IPath>();
        A.CallTo(() => fileSystem.Path).Returns(_path);

        A.CallTo(() => _path.Combine(A<string>._, A<string>._))
            .ReturnsLazily(call => Path.Combine(call.GetArgument<string>(0) ?? string.Empty,
                call.GetArgument<string>(1) ?? string.Empty));
    }

    [Fact]
    public async Task WriteAsync_WithFileNameAndOptions_CallsSerializerAndWritesContentToCorrectFile()
    {
        // Arrange
        const string optionName = "testFile";
        const string serializedOptions = "SerializedOptions";

        var options = new TestOptions { Name = "Test" };

        A.CallTo(() => _optionsSerializer.Serialize(options)).Returns(serializedOptions);

        _storageProvider.DirectoryPath = "TestDirectory";

        // Act
        await _storageProvider.WriteAsync(optionName, options);

        // Assert
        A.CallTo(() => _optionsSerializer.Serialize(options))
            .MustHaveHappenedOnceExactly();

        A.CallTo(() =>
                _file.WriteAllTextAsync(Path.Combine("TestDirectory", $"{optionName}.options"),
                    serializedOptions,
                    CancellationToken.None))
            .MustHaveHappenedOnceExactly();

        A.CallTo(() => _optionsCache.TryRemove(optionName))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void Write_WithFileNameAndOptions_CallsSerializerAndWritesContentToCorrectFile()
    {
        // Arrange
        const string optionName = "testFile";
        const string serializedOptions = "SerializedOptions";

        var options = new TestOptions { Name = "Test" };

        A.CallTo(() => _optionsSerializer.Serialize(options)).Returns(serializedOptions);

        _storageProvider.DirectoryPath = "TestDirectory";

        // Act
        _storageProvider.Write(optionName, options);

        // Assert
        A.CallTo(() => _optionsSerializer.Serialize(options))
            .MustHaveHappenedOnceExactly();

        A.CallTo(() =>
                _file.WriteAllText(Path.Combine("TestDirectory", $"{optionName}.options"), serializedOptions))
            .MustHaveHappenedOnceExactly();

        A.CallTo(() => _optionsCache.TryRemove(optionName))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task ReadAsync_WithFileNameAndOptions_CallsSerializerAndReadsContentFromCorrectFile()
    {
        // Arrange
        const string serializedOptions = "SerializedOptions";

        var options = new TestOptions { Name = "Test" };

        A.CallTo(() =>
                _file.ReadAllTextAsync(Path.Combine("TestDirectory", "testFile.options"),
                    CancellationToken.None))
            .Returns(serializedOptions);

        _storageProvider.DirectoryPath = "TestDirectory";

        // Act
        await _storageProvider.ReadAsync("testFile", options);

        // Assert
        A.CallTo(() =>
                _file.ReadAllTextAsync(Path.Combine("TestDirectory", "testFile.options"),
                    CancellationToken.None))
            .MustHaveHappenedOnceExactly();

        A.CallTo(() => _optionsSerializer.Deserialize(serializedOptions, options))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void Read_WithFileNameAndOptions_CallsSerializerAndReadsContentFromCorrectFile()
    {
        // Arrange
        const string serializedOptions = "SerializedOptions";

        var options = new TestOptions { Name = "Test" };

        A.CallTo(() => _file.ReadAllText(Path.Combine("TestDirectory", "testFile.options")))
            .Returns(serializedOptions);

        _storageProvider.DirectoryPath = "TestDirectory";

        // Act
        _storageProvider.Read("testFile", options);

        // Assert
        A.CallTo(() => _file.ReadAllText(Path.Combine("TestDirectory", "testFile.options")))
            .MustHaveHappenedOnceExactly();

        A.CallTo(() => _optionsSerializer.Deserialize(serializedOptions, options))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void Read_WithConvertNameToFileName_CallsSerializerAndReadsContentFromCorrectFile()
    {
        // Arrange
        const string serializedOptions = "SerializedOptions";

        var options = new TestOptions { Name = "Test" };

        A.CallTo(() => _file.ReadAllText(Path.Combine("TestDirectory", "testFile1234.data")))
            .Returns(serializedOptions);

        _storageProvider.DirectoryPath = "TestDirectory";
        _storageProvider.ConvertNameToFileName = name => $"{name}1234.data";

        // Act
        _storageProvider.Read("testFile", options);

        // Assert
        A.CallTo(() => _file.ReadAllText(Path.Combine("TestDirectory", "testFile1234.data"))
        ).MustHaveHappenedOnceExactly();

        A.CallTo(() => _optionsSerializer.Deserialize(serializedOptions, options))
            .MustHaveHappenedOnceExactly();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Read_DefaultFileNameSetAndNoName_CallsSerializerAndReadsContentFromCorrectFile(string? name)
    {
        // Arrange
        const string serializedOptions = "SerializedOptions";

        var options = new TestOptions { Name = "Test" };

        A.CallTo(() => _file.ReadAllText(Path.Combine("TestDirectory", "default.options")))
            .Returns(serializedOptions);

        _storageProvider.DirectoryPath = "TestDirectory";
        _storageProvider.DefaultFileName = "default";

        // Act
        _storageProvider.Read(name, options);

        // Assert
        A.CallTo(() => _file.ReadAllText(Path.Combine("TestDirectory", "default.options")))
            .MustHaveHappenedOnceExactly();

        A.CallTo(() => _optionsSerializer.Deserialize(serializedOptions, options))
            .MustHaveHappenedOnceExactly();
    }

    [Theory]
    [InlineData("file1234", new[] { 'f', 'i', 'l' }, "_", "___e1234")]
    public void Read_InvalidFileNameCharsInFileName_CallsSerializerAndReadsContentFromCorrectFile(
        string name, char[] invalidChars, string replacement, string expected)
    {
        // Arrange
        const string serializedOptions = "SerializedOptions";

        var options = new TestOptions { Name = "Test" };

        _storageProvider.DirectoryPath = "TestDirectory";
        _storageProvider.InvalidCharReplacement = replacement;

        A.CallTo(() => _file.ReadAllText(Path.Combine("TestDirectory", $"{expected}.options")))
            .Returns(serializedOptions);

        A.CallTo(() => _path.GetInvalidFileNameChars())
            .Returns(invalidChars);

        // Act
        _storageProvider.Read(name, options);

        // Assert
        A.CallTo(() => _file.ReadAllText(Path.Combine("TestDirectory", $"{expected}.options")))
            .MustHaveHappenedOnceExactly();

        A.CallTo(() => _optionsSerializer.Deserialize(serializedOptions, options))
            .MustHaveHappenedOnceExactly();
    }
}
