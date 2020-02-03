using System;
using System.Threading;
using CreativeCoders.Core.Threading;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Threading
{
    public class LockSlimLockingMechanismTests
    {
        [Fact]
        public void CtorTest()
        {
            var _ = new LockSlimLockingMechanism();
        }

        [Fact]
        public void CtorTestNull()
        {
            Assert.Throws<ArgumentNullException>(() => new LockSlimLockingMechanism(null));
        }

        [Fact]
        public void CtorTestReaderWriterLockSlim()
        {
            var _ = new LockSlimLockingMechanism(new ReaderWriterLockSlim());
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
    }
}