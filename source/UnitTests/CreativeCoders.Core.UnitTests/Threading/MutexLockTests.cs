using System;
using System.Threading.Tasks;
using CreativeCoders.Core.Threading;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Threading
{
    public class MutexLockTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void CtorTestAssert(string value)
        {
            Assert.Throws<ArgumentException>(() => new MutexLock(value));
        }

        [Fact]
        public void CtorTest()
        {
            const string mutexName = "test1";
            var mutex = new MutexLock(mutexName);

            var task = Task.Run(() => CreateMutexLock(mutexName));

            Task.Delay(5000);

            Assert.False(task.IsCompleted);

            mutex.Dispose();

            task.Wait();

            Assert.True(task.IsCompleted);

            Assert.NotNull(mutex);
        }

        private static void CreateMutexLock(string mutexName)
        {
            var mutexLock = new MutexLock(mutexName);
            mutexLock.Dispose();
        }
    }
}