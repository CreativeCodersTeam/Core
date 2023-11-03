using CreativeCoders.Core.Enums;
using FluentAssertions;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Enums;

public class EnumExtensionsTests
{
    [Theory]
    [InlineData(TestEnumWithFlags.Test1 | TestEnumWithFlags.Item,
        TestEnumWithFlags.Test1, TestEnumWithFlags.Item)]
    [InlineData((TestEnumWithFlags)0)]
    public void Test(TestEnumWithFlags flags, params TestEnumWithFlags[] expected)
    {
        // Act
        var actualValues = flags.EnumerateFlags();

        // Assert
        actualValues
            .Should()
            .BeEquivalentTo(expected);
    }
}
