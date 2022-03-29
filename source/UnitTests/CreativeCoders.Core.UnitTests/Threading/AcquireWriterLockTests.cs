using System;
using System.Threading;
using System.Threading.Tasks;
using CreativeCoders.Core.Threading;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Threading;

[Collection("Locking")]
public class AcquireWriterLockTests
{
    [Fact]
    public void AcquireWriterLockCtorTest()
    {
        using var slimLock = new ReaderWriterLockSlim();
        using var _ = new AcquireWriterLock(slimLock);

        Assert.Throws<ArgumentNullException>(() => new AcquireWriterLock(null));
    }

    [Fact]
    public void AcquireWriterLockTestUsing()
    {
        using var slimLock = new ReaderWriterLockSlim();

        using (new AcquireWriterLock(slimLock))
        {
            Assert.True(slimLock.IsWriteLockHeld);
            Assert.False(slimLock.IsReadLockHeld);
        }

        Assert.False(slimLock.IsReadLockHeld);
        Assert.False(slimLock.IsWriteLockHeld);
    }

    [Fact]
    public async Task AcquireWriterLockTestLockFailed()
    {
        var slimLock = new ReaderWriterLockSlim();

        slimLock.EnterReadLock();

        await Task.Run(() =>
                // ReSharper disable once AccessToDisposedClosure
                Assert.Throws<AcquireLockFailedException>(() => new AcquireWriterLock(slimLock, 1)))
            .ConfigureAwait(true);
    }
}