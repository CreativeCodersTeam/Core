using System;
using System.Threading;
using System.Threading.Tasks;
using CreativeCoders.Core.Threading;
using Xunit;
using Xunit.Sdk;

namespace CreativeCoders.Core.UnitTests.Threading;

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
    public async Task CtorTest()
    {
        const string mutexName = "test1";
        var mutex = new MutexLock(mutexName);

        var task = Task.Run(() => CreateMutexLock(mutexName));

        var waitTask = Task.Run(async () => await Task.Delay(5000));

        while (!waitTask.IsCompleted) {}

        Assert.False(task.IsCompleted);

        mutex.Dispose();

        await task.WaitAsync(CancellationToken.None);

        Assert.True(task.IsCompleted);

        Assert.NotNull(mutex);
    }

    private static void CreateMutexLock(string mutexName)
    {
        var mutexLock = new MutexLock(mutexName);
        mutexLock.Dispose();
    }
}
