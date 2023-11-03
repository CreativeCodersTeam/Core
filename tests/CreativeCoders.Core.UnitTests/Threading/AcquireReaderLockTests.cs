using System;
using System.Threading;
using System.Threading.Tasks;
using CreativeCoders.Core.Threading;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Threading;

[Collection("Locking")]
public class AcquireReaderLockTests
{
    [Fact]
    public void AcquireReaderLockCtorTest()
    {
        using var slimLock = new ReaderWriterLockSlim();
        using var _ = new AcquireReaderLock(slimLock);

        Assert.Throws<ArgumentNullException>(() => new AcquireReaderLock(null));
    }

    [Fact]
    public void AcquireReaderLockTestUsing()
    {
        using var slimLock = new ReaderWriterLockSlim();

        using (new AcquireReaderLock(slimLock))
        {
            Assert.True(slimLock.IsReadLockHeld);
            Assert.False(slimLock.IsWriteLockHeld);
        }

        Assert.False(slimLock.IsReadLockHeld);
        Assert.False(slimLock.IsWriteLockHeld);
    }

    [Fact]
    public async Task AcquireReaderLockTestLockFailed()
    {
        var slimLock = new ReaderWriterLockSlim();

        slimLock.EnterWriteLock();

        await Task.Run(() =>
            Assert.Throws<AcquireLockFailedException>(() => new AcquireReaderLock(slimLock, 1)));
    }
}
