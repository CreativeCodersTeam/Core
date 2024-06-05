using System.Diagnostics.CodeAnalysis;
using CreativeCoders.Core.Text;
using FluentAssertions;

namespace CreativeCoders.UnitTests.UnitTests;

[SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
public class ObjectExtensionsTests
{
    [Fact]
    public void IsEquivalentTo_WithEquivalentObjects_ReturnsTrue()
    {
        // Arrange
        var obj1 = new { Prop1 = "value1", Prop2 = 123 };
        var obj2 = new { Prop1 = "value1", Prop2 = 123 };

        // Act
        var result = obj1.IsEquivalentTo(obj2);

        // Assert
        result
            .Should()
            .BeTrue();
    }

    [Fact]
    public void IsEquivalentTo_WithNonEquivalentObjects_ReturnsFalse()
    {
        // Arrange
        var obj1 = new TextSpan(1, 2);
        var obj2 = new TextSpan(2, 2);

        // Act
        var result = obj1.IsEquivalentTo(obj2);

        // Assert
        result
            .Should()
            .BeFalse();
    }

    [Fact]
    public void IsEquivalentTo_WithNullSecondObject_ThrowsArgumentNullException()
    {
        // Arrange
        var obj1 = new { Prop1 = "value1", Prop2 = 123 };

        // Act
        Action act = () => obj1.IsEquivalentTo(null!);

        // Assert
        act
            .Should()
            .Throw<ArgumentNullException>();
    }

    [Fact]
    public void IsEquivalentTo_WithNullFirstObject_ThrowsArgumentNullException()
    {
        // Arrange
        var obj2 = new { Prop1 = "value1", Prop2 = 123 };

        // Act
        Action act = () => ((object)null!).IsEquivalentTo(obj2);

        // Assert
        act
            .Should()
            .Throw<ArgumentNullException>();
    }
}
