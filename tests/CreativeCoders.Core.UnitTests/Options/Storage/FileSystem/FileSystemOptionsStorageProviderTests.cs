using System.IO;
using System.IO.Abstractions;
using System.Threading;
using System.Threading.Tasks;
using CreativeCoders.Core.IO;
using CreativeCoders.Options.Core;
using CreativeCoders.Options.Storage.FileSystem;
using FakeItEasy;
using Xunit;

#nullable enable

namespace CreativeCoders.Core.UnitTests.Options.Storage.FileSystem;

[Collection("FileSys")]
public class FileSystemOptionsStorageProviderTests
{
    private readonly IFile _file;

    private readonly IOptionsStorageDataSerializer _optionsSerializer;

    private readonly FileSystemOptionsStorageProvider<TestOptions> _storageProvider;

    public FileSystemOptionsStorageProviderTests()
    {
        _optionsSerializer = A.Fake<IOptionsStorageDataSerializer>();
        _storageProvider = new FileSystemOptionsStorageProvider<TestOptions>(_optionsSerializer);

        var fileSystem = A.Fake<IFileSystemEx>();
        FileSys.InstallFileSystemSupport(fileSystem);

        _file = A.Fake<IFile>();
        A.CallTo(() => fileSystem.File).Returns(_file);

        var path = A.Fake<IPath>();
        A.CallTo(() => fileSystem.Path).Returns(path);

        A.CallTo(() => path.Combine(A<string>._, A<string>._))
            .ReturnsLazily(call => Path.Combine(call.GetArgument<string>(0) ?? string.Empty,
                call.GetArgument<string>(1) ?? string.Empty));
    }

    [Fact]
    public async Task WriteAsync_WithFileNameAndOptions_CallsSerializerAndWritesContentToCorrectFile()
    {
        // Arrange
        const string serializedOptions = "SerializedOptions";

        var options = new TestOptions { Name = "Test" };

        A.CallTo(() => _optionsSerializer.Serialize(options)).Returns(serializedOptions);

        _storageProvider.DirectoryPath = "TestDirectory";

        // Act
        await _storageProvider.WriteAsync("testFile", options);

        // Assert
        A.CallTo(() => _optionsSerializer.Serialize(options))
            .MustHaveHappenedOnceExactly();

        A.CallTo(() =>
                _file.WriteAllTextAsync(Path.Combine("TestDirectory", "testFile.options"), serializedOptions,
                    CancellationToken.None))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void Write_WithFileNameAndOptions_CallsSerializerAndWritesContentToCorrectFile()
    {
        // Arrange
        const string serializedOptions = "SerializedOptions";

        var options = new TestOptions { Name = "Test" };

        A.CallTo(() => _optionsSerializer.Serialize(options)).Returns(serializedOptions);

        _storageProvider.DirectoryPath = "TestDirectory";

        // Act
        _storageProvider.Write("testFile", options);

        // Assert
        A.CallTo(() => _optionsSerializer.Serialize(options))
            .MustHaveHappenedOnceExactly();

        A.CallTo(() =>
                _file.WriteAllText(Path.Combine("TestDirectory", "testFile.options"), serializedOptions))
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
}
