using System;
using System.Threading;
using System.Threading.Tasks;
using CreativeCoders.Core.Threading;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Threading
{
    public class AcquireWriterLockTests
    {
        [Fact]
        public void AcquireWriterLockCtorTest()
        {
            var slimLock = new ReaderWriterLockSlim();
            var _ = new AcquireWriterLock(slimLock);

            Assert.Throws<ArgumentNullException>(() => new AcquireWriterLock(null));
        }

        [Fact]
        public void AcquireWriterLockTestUsing()
        {
            var slimLock = new ReaderWriterLockSlim();
            using (new AcquireWriterLock(slimLock))
            {
                Assert.True(slimLock.IsWriteLockHeld);
                Assert.False(slimLock.IsReadLockHeld);
            }
            Assert.False(slimLock.IsReadLockHeld);
            Assert.False(slimLock.IsWriteLockHeld);
        }

        [Fact]
        public void AcquireWriterLockTestLockFailed()
        {
            var slimLock = new ReaderWriterLockSlim();
            slimLock.EnterReadLock();
            var task = Task.Run(() => Assert.Throws<AcquireLockFailedException>(() => new AcquireWriterLock(slimLock, 1)));
            task.Wait();
        }
    }
}