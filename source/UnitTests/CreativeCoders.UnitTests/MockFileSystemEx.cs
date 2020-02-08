using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using CreativeCoders.Core.IO;

namespace CreativeCoders.UnitTests
{
    public class MockFileSystemEx : MockFileSystem, IFileSystemEx
    {
        public MockFileSystemEx()
        {
        }

        public MockFileSystemEx(IDictionary<string, MockFileData> files, string currentDirectory) : base(files, currentDirectory)
        {
        }
        
        public void Install()
        {
            FileSys.InstallFileSystemSupport(this);
        }

        public FileSystemWatcherBase CreateFileSystemWatcher()
        {
            return new MockFileSystemWatcher();
        }

        public FileSystemWatcherBase CreateFileSystemWatcher(string path)
        {
            return new MockFileSystemWatcher();
        }

        public FileSystemWatcherBase CreateFileSystemWatcher(string path, string filter)
        {
            return new MockFileSystemWatcher();
        }
    }
}
