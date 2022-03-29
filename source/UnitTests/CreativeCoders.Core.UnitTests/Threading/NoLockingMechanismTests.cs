using CreativeCoders.Core.Threading;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Threading;

public class NoLockingMechanismTests
{
    [Fact]
    public void Read_WithInnerWrite_PassedWithoutProblems()
    {
        var value = 1;
        var actual = 1;

        var lockingMechanism = new NoLockingMechanism();

        lockingMechanism.Read(
            () =>
            {
                lockingMechanism.Write(() => { value = 2; });
                actual = value;
            });

        Assert.Equal(value, actual);
    }

    [Fact]
    public void Read_WithResultWithInnerWrite_PassedWithoutProblemsAndReturnsCorrectResult()
    {
        var value = 1;
        var actual = 1;

        var lockingMechanism = new NoLockingMechanism();

        var result = lockingMechanism.Read(
            () =>
            {
                lockingMechanism.Write(() => { value = 2; });
                actual = value;
                return value;
            });

        Assert.Equal(value, actual);
        Assert.Equal(value, result);
    }

    [Fact]
    public void Write_WithInnerRead_PassedWithoutProblems()
    {
        var value = 1;
        var actual = 1;

        var lockingMechanism = new NoLockingMechanism();

        lockingMechanism.Write(
            () =>
            {
                value = 2;
                lockingMechanism.Read(() => { actual = value; });
            });

        Assert.Equal(value, actual);
    }

    [Fact]
    public void Write_WithResultWithInnerRead_PassedWithoutProblemsAndReturnsCorrectResult()
    {
        var value = 1;
        var actual = 1;

        var lockingMechanism = new NoLockingMechanism();

        var result = lockingMechanism.Write(
            () =>
            {
                value = 2;
                lockingMechanism.Read(() => { actual = value; });
                return actual;
            });

        Assert.Equal(value, actual);
        Assert.Equal(value, result);
    }
}