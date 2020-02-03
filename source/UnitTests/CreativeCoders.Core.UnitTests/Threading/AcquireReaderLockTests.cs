using System;
using System.Threading;
using System.Threading.Tasks;
using CreativeCoders.Core.Threading;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Threading
{
    public class AcquireReaderLockTests
    {
        [Fact]
        public void AcquireReaderLockCtorTest()
        {
            var slimLock = new ReaderWriterLockSlim();
            var _ = new AcquireReaderLock(slimLock);

            Assert.Throws<ArgumentNullException>(() => new AcquireReaderLock(null));
        }

        [Fact]
        public void AcquireReaderLockTestUsing()
        {
            var slimLock = new ReaderWriterLockSlim();
            using (new AcquireReaderLock(slimLock))
            {
                Assert.True(slimLock.IsReadLockHeld);
                Assert.False(slimLock.IsWriteLockHeld);
            }
            Assert.False(slimLock.IsReadLockHeld);
            Assert.False(slimLock.IsWriteLockHeld);
        }

        [Fact]
        public void AcquireReaderLockTestLockFailed()
        {
            var slimLock = new ReaderWriterLockSlim();
            slimLock.EnterWriteLock();
            var task = Task.Run(() => Assert.Throws<AcquireLockFailedException>(() => new AcquireReaderLock(slimLock, 1)));
            task.Wait();
        }


    }
}