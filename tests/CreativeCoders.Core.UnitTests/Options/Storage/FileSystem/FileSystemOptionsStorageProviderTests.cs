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

    private readonly IFileSystemEx _fileSystem;

    private readonly IOptionsStorageDataSerializer _optionsSerializer;

    private readonly IPath _path;

    private readonly FileSystemOptionsStorageProvider<TestOptions> _storageProvider;

    public FileSystemOptionsStorageProviderTests()
    {
        _optionsSerializer = A.Fake<IOptionsStorageDataSerializer>();
        _storageProvider = new FileSystemOptionsStorageProvider<TestOptions>(_optionsSerializer);

        _fileSystem = A.Fake<IFileSystemEx>();
        FileSys.InstallFileSystemSupport(_fileSystem);

        _file = A.Fake<IFile>();
        A.CallTo(() => _fileSystem.File).Returns(_file);

        _path = A.Fake<IPath>();
        A.CallTo(() => _fileSystem.Path).Returns(_path);

        A.CallTo(() => _path.Combine(A<string>._, A<string>._))
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

        _storageProvider.DirectoryPath = @"C:\TestDirectory";

        // Act
        await _storageProvider.WriteAsync("testFile", options);

        // Assert
        A.CallTo(() => _optionsSerializer.Serialize(options))
            .MustHaveHappenedOnceExactly();

        A.CallTo(() =>
                _file.WriteAllTextAsync(@"C:\TestDirectory\testFile.options", serializedOptions,
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

        _storageProvider.DirectoryPath = @"C:\TestDirectory";

        // Act
        _storageProvider.Write("testFile", options);

        // Assert
        A.CallTo(() => _optionsSerializer.Serialize(options))
            .MustHaveHappenedOnceExactly();

        A.CallTo(() =>
                _file.WriteAllText(@"C:\TestDirectory\testFile.options", serializedOptions))
            .MustHaveHappenedOnceExactly();
    }
}
