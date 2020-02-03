using CreativeCoders.Core.Threading;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Threading
{
    public class SynchronizedValueTests
    {
        [Fact]
        public void ValueGetTest()
        {
            var value = new SynchronizedValue<int> {Value = 12345};
            Assert.Equal(12345, value.Value);
        }

        [Fact]
        public void ValueGetTestWithLock()
        {
            var value = new SynchronizedValue<int>(new LockLockingMechanism()) {Value = 12345};
            Assert.Equal(12345, value.Value);
        }
    }
}