using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using CreativeCoders.Core.IO;
using JetBrains.Annotations;

namespace CreativeCoders.UnitTests;

[PublicAPI]
public class MockFileSystemEx : MockFileSystem, IFileSystemEx
{
    public MockFileSystemEx() { }

    public MockFileSystemEx(IDictionary<string, MockFileData> files, string currentDirectory) : base(files,
        currentDirectory) { }

    public void Install()
    {
        FileSys.InstallFileSystemSupport(this);
    }

    public FileSystemWatcherBase CreateFileSystemWatcher()
    {
        return new MockFileSystemWatcher(this);
    }

    public FileSystemWatcherBase CreateFileSystemWatcher(string path)
    {
        return new MockFileSystemWatcher(this);
    }

    public FileSystemWatcherBase CreateFileSystemWatcher(string path, string filter)
    {
        return new MockFileSystemWatcher(this);
    }
}
