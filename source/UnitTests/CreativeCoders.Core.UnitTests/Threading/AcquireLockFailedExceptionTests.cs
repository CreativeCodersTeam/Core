using CreativeCoders.Core.Threading;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Threading;

public class AcquireLockFailedExceptionTests
{
    [Fact]
    public void CtorTest()
    {
        // ReSharper disable once ObjectCreationAsStatement
        new AcquireLockFailedException("test", 1234);
    }

    [Fact]
    public void CtorTestTimeout()
    {
        var ex = new AcquireLockFailedException("test", 1234);
        Assert.Equal(1234, ex.Timeout);
    }

    [Fact]
    public void CtorTestThrow()
    {
        Assert.Throws<AcquireLockFailedException>(ThrowExceptionOnCall);
    }

    private static void ThrowExceptionOnCall()
    {
        throw new AcquireLockFailedException("test", 1234);
    }
}