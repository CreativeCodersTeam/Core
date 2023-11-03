using System;
using CreativeCoders.Core.Threading;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Threading;

public class LockLockingMechanismTests
{
    [Fact]
    public void CtorTest()
    {
        var _ = new LockLockingMechanism();
    }

    [Fact]
    public void CtorTestNull()
    {
        Assert.Throws<ArgumentNullException>(() => new LockLockingMechanism(null));
    }

    [Fact]
    public void CtorTestReaderWriterLockSlim()
    {
        var _ = new LockLockingMechanism(new object());
    }

    [Fact]
    public void ReadActionTest()
    {
        var executed = false;

        var slimLock = new LockLockingMechanism();
        var action = new Action(() => executed = true);
        slimLock.Read(action);

        Assert.True(executed);
    }

    [Fact]
    public void WriteActionTest()
    {
        var executed = false;

        var slimLock = new LockLockingMechanism();
        var action = new Action(() => executed = true);
        slimLock.Write(action);

        Assert.True(executed);
    }
}
