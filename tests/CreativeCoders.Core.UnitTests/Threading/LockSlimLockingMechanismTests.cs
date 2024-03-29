﻿using System;
using System.Threading;
using CreativeCoders.Core.Threading;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Threading;

public class LockSlimLockingMechanismTests
{
    [Fact]
    public void CtorTest()
    {
        _ = new LockSlimLockingMechanism();
    }

    [Fact]
    public void CtorTestNull()
    {
        Assert.Throws<ArgumentNullException>(() => new LockSlimLockingMechanism(null));
    }

    [Fact]
    public void CtorTestReaderWriterLockSlim()
    {
        _ = new LockSlimLockingMechanism(new ReaderWriterLockSlim());
    }

    [Fact]
    public void ReadActionTest()
    {
        var executed = false;

        var slimLock = new LockSlimLockingMechanism();
        var action = new Action(() => executed = true);
        slimLock.Read(action);

        Assert.True(executed);
    }

    [Fact]
    public void WriteActionTest()
    {
        var executed = false;

        var slimLock = new LockSlimLockingMechanism();
        var action = new Action(() => executed = true);
        slimLock.Write(action);

        Assert.True(executed);
    }

    [Fact]
    public void UpgradeableRead_WithWriteUpgrade_CalledCorrectAndLockModesAreAlsoCorrect()
    {
        var executed = false;

        var slimLock = new ReaderWriterLockSlim();

        var lockingMechanism = new LockSlimLockingMechanism(slimLock);

        lockingMechanism.UpgradeableRead(useWriteLock =>
        {
            Assert.True(slimLock.IsUpgradeableReadLockHeld);
            Assert.False(slimLock.IsWriteLockHeld);

            using (useWriteLock())
            {
                Assert.True(slimLock.IsWriteLockHeld);
                executed = true;
            }
        });

        Assert.True(executed);
    }

    [Fact]
    public void UpgradeableReadWithResult_WithWriteUpgrade_CalledCorrectAndLockModesAreAlsoCorrect()
    {
        const string expectedResult = "Test text";

        var executed = false;

        var slimLock = new ReaderWriterLockSlim();

        var lockingMechanism = new LockSlimLockingMechanism(slimLock);

        var result = lockingMechanism.UpgradeableRead(useWriteLock =>
        {
            Assert.True(slimLock.IsUpgradeableReadLockHeld);
            Assert.False(slimLock.IsWriteLockHeld);

            using (useWriteLock())
            {
                Assert.True(slimLock.IsWriteLockHeld);
                executed = true;
                return expectedResult;
            }
        });

        Assert.True(executed);
        Assert.Equal(expectedResult, result);
    }
}
