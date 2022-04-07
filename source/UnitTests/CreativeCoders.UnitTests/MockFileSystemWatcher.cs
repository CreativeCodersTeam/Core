using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.IO.Abstractions;

namespace CreativeCoders.UnitTests;

public class MockFileSystemWatcher : FileSystemWatcherBase
{
    public MockFileSystemWatcher()
    {
        Filters = new Collection<string>();
    }

    public override void BeginInit()
    {
        throw new System.NotImplementedException();
    }

    public override void EndInit()
    {
        throw new System.NotImplementedException();
    }

    public override WaitForChangedResult WaitForChanged(WatcherChangeTypes changeType)
    {
        return new();
    }

    public override WaitForChangedResult WaitForChanged(WatcherChangeTypes changeType, int timeout)
    {
        return new();
    }

    public override bool IncludeSubdirectories { get; set; }

    public override bool EnableRaisingEvents { get; set; }

    public override string Filter { get; set; }

    public override Collection<string> Filters { get; }

    public override int InternalBufferSize { get; set; }

    public override NotifyFilters NotifyFilter { get; set; }

    public override string Path { get; set; }

    public override ISite Site { get; set; }

    public override ISynchronizeInvoke SynchronizingObject { get; set; }
}
