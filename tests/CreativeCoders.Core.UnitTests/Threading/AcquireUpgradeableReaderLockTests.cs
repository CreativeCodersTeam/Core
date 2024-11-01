using System.Threading;
using System.Threading.Tasks;
using CreativeCoders.Core.Threading;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Threading;

[Collection("Locking")]
public class AcquireUpgradeableReaderLockTests
{
    [Fact]
    public void AcquireUpgradeableReaderLock_Using_LockIsInReadMode()
    {
        using var slimLock = new ReaderWriterLockSlim();

        using (new AcquireUpgradeableReaderLock(slimLock))
        {
            Assert.True(slimLock.IsUpgradeableReadLockHeld);
            Assert.False(slimLock.IsWriteLockHeld);
        }
    }

    [Fact]
    public void UseWriteLock_UpgradeFromReadToWrite_Success()
    {
        using var slimLock = new ReaderWriterLockSlim();

        using var upgradeableLock = new AcquireUpgradeableReaderLock(slimLock);

        Assert.True(slimLock.IsUpgradeableReadLockHeld);
        Assert.False(slimLock.IsWriteLockHeld);

        using (upgradeableLock.UseWriteLock())
        {
            Assert.True(slimLock.IsWriteLockHeld);
        }
    }

    [Fact]
    public void Ctor_EnterUpgradeableWhenLockIsAlreadyInWrite_ThrowsException()
    {
        var slimLock = new ReaderWriterLockSlim();

        slimLock.EnterWriteLock();

        var thread = new Thread(() =>
            Assert.Throws<AcquireLockFailedException>(() =>
                new AcquireUpgradeableReaderLock(slimLock, 1)));

        thread.Start();

        thread.Join();
    }

    [Fact]
    public void Ctor_EnterUpgradeableWhenLockIsAlreadyInRead_LockIsInUpgradeableMode()
    {
        var executed = false;

        var slimLock = new ReaderWriterLockSlim();

        slimLock.EnterReadLock();

        var thread = new Thread(() =>
        {
            using (new AcquireUpgradeableReaderLock(slimLock))
            {
                Assert.True(slimLock.IsUpgradeableReadLockHeld);
                executed = true;
            }
        });

        thread.Start();

        thread.Join();

        Assert.True(executed);
    }

    [Fact]
    public void Ctor_EnterUpgradeableWhenLockIsAlreadyInReadAndUpgrade_ThrowsException()
    {
        var executed = false;

        var slimLock = new ReaderWriterLockSlim();

        slimLock.EnterReadLock();

        var thread = new Thread(() =>
        {
            using (var upgradeableLock = new AcquireUpgradeableReaderLock(slimLock))
            {
                Assert.Throws<AcquireLockFailedException>(() =>
                {
                    executed = true;
                    _ = upgradeableLock.UseWriteLock(1);
                });
            }
        });

        thread.Start();

        thread.Join();

        Assert.True(executed);
    }
}
