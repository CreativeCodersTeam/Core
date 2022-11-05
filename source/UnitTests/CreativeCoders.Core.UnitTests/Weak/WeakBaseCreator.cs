using System;
using System.Diagnostics.CodeAnalysis;
using CreativeCoders.Core.Weak;

namespace CreativeCoders.Core.UnitTests.Weak;

public class WeakBaseCreator
{
    public WeakBase<Action> CreateWeakBase()
    {
        var writer = new TestConsoleWriter();

        var weakAction =
            new WeakBase<Action>(this, () => writer.Write("Test"), KeepOwnerAliveMode.NotKeepAlive);
        return weakAction;
    }

    // ReSharper disable once MemberCanBeMadeStatic.Global
    [SuppressMessage("Performance", "CA1822")]
    public WeakBase<Action> CreateWeakBaseWithoutOwner()
    {
        var writer = new TestConsoleWriter();

        var weakAction =
            new WeakBase<Action>(null, () => writer.Write("Test"), KeepOwnerAliveMode.NotKeepAlive);
        return weakAction;
    }
}
