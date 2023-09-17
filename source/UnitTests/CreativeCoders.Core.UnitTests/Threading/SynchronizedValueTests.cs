using CreativeCoders.Core.Threading;
using FluentAssertions;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Threading;

public class SynchronizedValueTests
{
    [Fact]
    public void ValueGetTest()
    {
        const int expectedValue = 12345;

        // Arrange & Act
        var value = new SynchronizedValue<int> { Value = expectedValue };

        // Assert
        value.Value
            .Should()
            .Be(expectedValue);
    }

    [Fact]
    public void ValueGetTestWithLock()
    {
        const int expectedValue = 12345;

        // Arrange & Act
        var value = new SynchronizedValue<int>(new LockLockingMechanism()) {Value = expectedValue};

        // Assert
        value.Value
            .Should()
            .Be(expectedValue);
    }
}
