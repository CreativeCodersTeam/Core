using CreativeCoders.SysConsole.Core.Abstractions;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace CreativeCoders.SysConsole;

public class SysConsoleExtensionsTests
{
    [Theory]
    [InlineData("1234", 1, "1234", "abcd")]
    [InlineData("abcd", 2, "1234", "abcd", "5678")]
    [InlineData("1234", 1, "1234")]
    [InlineData("5678", 3, "1234", "abcd", "5678")]
    public void SelectItem_ExistingItemIsSelected_ReturnsSelectedItem(
        string expectedItem, int selectionIndex, params string[] items)
    {
        var console = A.Fake<ISysConsole>();

        A
            .CallTo(() => console.ReadLine())
            .Returns(selectionIndex.ToString());

        // Act
        var selectedItem = console.SelectItem(items);

        // Assert
        selectedItem
            .Should()
            .Be(expectedItem);

        A
            .CallTo(() => console.WriteLine(A<string>.Ignored))
            .MustHaveHappened(items.Length, Times.Exactly);
    }

    [Theory]
    [InlineData(null, "2", "Test")]
    [InlineData(0, "2", 1234)]
    [InlineData(false, "2", true)]
    [InlineData(false, "a", true)]
    public void SelectItem_NotExistingItemIsSelected_ReturnsDefault(
        object expectedItem, string selectionIndex, params object[] items)
    {
        var console = A.Fake<ISysConsole>();

        A
            .CallTo(() => console.ReadLine())
            .Returns(selectionIndex);

        // Act
        var selectedItem = console.SelectItem(items, expectedItem);

        // Assert
        selectedItem
            .Should()
            .Be(expectedItem);
    }
}
