using System.Threading;
using System.Threading.Tasks;
using CreativeCoders.Core.Threading;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Threading
{
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
        public async Task Ctor_EnterUpgradeableWhenLockIsAlreadyInWrite_ThrowsException()
        {
            using var slimLock = new ReaderWriterLockSlim();

            try {
                slimLock.EnterWriteLock();

                await Task.Run(() =>
                {
                    Assert.Throws<AcquireLockFailedException>(() => new AcquireUpgradeableReaderLock(slimLock, 1));
                });
            }
            finally
            {
                slimLock.ExitWriteLock();
            }
            
        }
        
        [Fact]
        public async Task Ctor_EnterUpgradeableWhenLockIsAlreadyInRead_LockIsInUpgradeableMode()
        {
            var executed = false;

            using var slimLock = new ReaderWriterLockSlim();

            try
            {
                slimLock.EnterReadLock();

                await Task.Run(() =>
                {
                    using (new AcquireUpgradeableReaderLock(slimLock))
                    {
                        Assert.True(slimLock.IsUpgradeableReadLockHeld);
                        executed = true;
                    }
                });
            }
            finally
            {
                slimLock.ExitReadLock();
            }
            
            Assert.True(executed);
        }
        
        [Fact]
        public async Task Ctor_EnterUpgradeableWhenLockIsAlreadyInReadAndUpgrade_ThrowsException()
        {
            var executed = false;

            using var slimLock = new ReaderWriterLockSlim();

            try {
                slimLock.EnterReadLock();

                await Task.Run(() =>
                {
                    using (var upgradeableLock = new AcquireUpgradeableReaderLock(slimLock))
                    {
                        Assert.Throws<AcquireLockFailedException>(() =>
                        {
                            executed = true;
                            var _ = upgradeableLock.UseWriteLock(1);
                        });

                    }
                });
            }
            finally
            {
                slimLock.ExitReadLock();
            }
            
            Assert.True(executed);
        }
    }
}
