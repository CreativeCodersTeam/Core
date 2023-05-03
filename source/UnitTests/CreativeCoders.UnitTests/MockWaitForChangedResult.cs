using System.IO;
using System.IO.Abstractions;
using JetBrains.Annotations;

namespace CreativeCoders.UnitTests;

[PublicAPI]
public class MockWaitForChangedResult : IWaitForChangedResult
{
    public WatcherChangeTypes ChangeType { get; init; }

    public string Name { get; init; }

    public string OldName { get; init; }

    public bool TimedOut { get; init; }
}
