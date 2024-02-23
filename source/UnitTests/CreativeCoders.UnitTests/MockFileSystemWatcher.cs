using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.IO.Abstractions;
using CreativeCoders.Core;
using JetBrains.Annotations;

namespace CreativeCoders.UnitTests;

[PublicAPI]
public class MockFileSystemWatcher : FileSystemWatcherBase
{
    public MockFileSystemWatcher(IFileSystem fileSystem)
    {
        FileSystem = Ensure.NotNull(fileSystem);

        Filters = new Collection<string>();
    }

    public override IContainer? Container { get; }

    public override IFileSystem FileSystem { get; }

    public override bool IncludeSubdirectories { get; set; }

    public override bool EnableRaisingEvents { get; set; }

    public override string Filter { get; set; } = string.Empty;

    public override Collection<string> Filters { get; }

    public override int InternalBufferSize { get; set; }

    public override NotifyFilters NotifyFilter { get; set; }

    public override string Path { get; set; } = string.Empty;

    public override ISite? Site { get; set; }

    public override ISynchronizeInvoke? SynchronizingObject { get; set; }

    public override void BeginInit()
    {
        throw new System.NotImplementedException();
    }

    public override void EndInit()
    {
        throw new System.NotImplementedException();
    }

    public override IWaitForChangedResult WaitForChanged(WatcherChangeTypes changeType)
    {
        return new MockWaitForChangedResult();
    }

    public override IWaitForChangedResult WaitForChanged(WatcherChangeTypes changeType, int timeout)
    {
        return new MockWaitForChangedResult();
    }
}
