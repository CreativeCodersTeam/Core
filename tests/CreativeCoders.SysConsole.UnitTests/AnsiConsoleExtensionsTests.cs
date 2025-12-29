using System.Diagnostics.CodeAnalysis;
using AwesomeAssertions;
using CreativeCoders.SysConsole.Core;
using FakeItEasy;
using Spectre.Console;
using Spectre.Console.Rendering;
using Xunit;

namespace CreativeCoders.SysConsole.UnitTests;

[SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
public class AnsiConsoleExtensionsTests
{
    [Fact]
    public void PrintTable_ItemsAndColumns_TableIsWrittenToConsole()
    {
        // Arrange
        var ansiConsole = A.Fake<IAnsiConsole>();

        var items = new[]
        {
            new TestItem { Name = "Item1", Value = 10 },
            new TestItem { Name = "Item2", Value = 20 }
        };

        var columns = new[]
        {
            new TableColumnDef<TestItem>(x => x.Name, "Name"),
            new TableColumnDef<TestItem>(x => x.Value, "Value")
        };

        Table? capturedTable = null;
        A.CallTo(() => ansiConsole.Write(A<IRenderable>.Ignored))
            .Invokes(call => capturedTable = call.Arguments.Get<Table>(0));

        // Act
        ansiConsole.PrintTable(items, columns);

        // Assert
        capturedTable.Should().NotBeNull();
        capturedTable!.Rows.Count.Should().Be(3); // 1 header separator row + 2 data rows
        capturedTable.ShowHeaders.Should().BeTrue();
    }

    [Fact]
    public void PrintTable_NoColumnTitles_HeadersAreNotShown()
    {
        // Arrange
        var ansiConsole = A.Fake<IAnsiConsole>();

        var items = new[]
        {
            new TestItem { Name = "Item1", Value = 10 }
        };

        var columns = new[]
        {
            new TableColumnDef<TestItem>(x => x.Name)
        };

        Table? capturedTable = null;
        A.CallTo(() => ansiConsole.Write(A<IRenderable>.Ignored))
            .Invokes(call => capturedTable = call.Arguments.Get<Table>(0));

        // Act
        ansiConsole.PrintTable(items, columns);

        // Assert
        capturedTable.Should().NotBeNull();
        capturedTable!.ShowHeaders.Should().BeFalse();
        capturedTable.Rows.Count.Should().Be(1); // Only data row
    }

    [Fact]
    public void PrintTable_WithConfigureTable_TableIsConfigured()
    {
        // Arrange
        var ansiConsole = A.Fake<IAnsiConsole>();

        var items = new[]
        {
            new TestItem { Name = "Item1", Value = 10 }
        };

        var columns = new[]
        {
            new TableColumnDef<TestItem>(x => x.Name, "Name")
        };

        Table? capturedTable = null;
        A.CallTo(() => ansiConsole.Write(A<IRenderable>.Ignored))
            .Invokes(call => capturedTable = call.Arguments.Get<Table>(0));

        // Act
        ansiConsole.PrintTable(items, columns, table => table.Title = new TableTitle("TestTitle"));

        // Assert
        capturedTable.Should().NotBeNull();
        capturedTable!.Title!.Text.Should().Be("TestTitle");
    }

    [Fact]
    public void PrintTable_WithColumnWidth_ColumnHasSpecifiedWidth()
    {
        // Arrange
        var ansiConsole = A.Fake<IAnsiConsole>();

        var items = new[]
        {
            new TestItem { Name = "Item1", Value = 10 }
        };

        var columns = new[]
        {
            new TableColumnDef<TestItem>(x => x.Name, "Name", width: 20)
        };

        Table? capturedTable = null;
        A.CallTo(() => ansiConsole.Write(A<IRenderable>.Ignored))
            .Invokes(call => capturedTable = call.Arguments.Get<Table>(0));

        // Act
        ansiConsole.PrintTable(items, columns);

        // Assert
        capturedTable.Should().NotBeNull();
        capturedTable!.Columns[0].Width.Should().Be(20);
    }

    [Fact]
    public void PrintTable_EmptyItems_OnlyHeaderRowsAreWritten()
    {
        // Arrange
        var ansiConsole = A.Fake<IAnsiConsole>();

        var items = Enumerable.Empty<TestItem>();

        var columns = new[]
        {
            new TableColumnDef<TestItem>(x => x.Name, "Name")
        };

        Table? capturedTable = null;
        A.CallTo(() => ansiConsole.Write(A<IRenderable>.Ignored))
            .Invokes(call => capturedTable = call.Arguments.Get<Table>(0));

        // Act
        ansiConsole.PrintTable(items, columns);

        // Assert
        capturedTable.Should().NotBeNull();
        capturedTable!.Rows.Count.Should().Be(1); // Only header separator row
    }

    [Fact]
    public void PrintTable_ColoredColumns_TableIsWrittenWithColorToConsole()
    {
        // Arrange
        var ansiConsole = A.Fake<IAnsiConsole>();

        var items = new[]
        {
            new TestItem { Name = "Item1", Value = 10 }
        };

        var columns = new[]
        {
            new TableColumnDef<TestItem>(x => x.Name, "Name", color: Color.Red),
            new TableColumnDef<TestItem>(x => x.Value, "Value", color: Color.Green)
        };

        Table? capturedTable = null;
        A.CallTo(() => ansiConsole.Write(A<IRenderable>.Ignored))
            .Invokes(call => capturedTable = call.Arguments.Get<Table>(0));

        // Act
        ansiConsole.PrintTable(items, columns);

        // Assert
        capturedTable.Should().NotBeNull();
        capturedTable!.Rows.Count.Should().Be(2);

        var row = (capturedTable.Rows as IReadOnlyList<TableRow>)[1];

        var col0 = row[0].GetSegments(AnsiConsole.Create(new AnsiConsoleSettings())).First();
        col0.Text
            .Should().Be("Item1");

        col0.Style.Foreground
            .Should().Be(Color.Red);

        var col1 = row[1].GetSegments(AnsiConsole.Create(new AnsiConsoleSettings())).First();
        col1.Text
            .Should().Be("10");

        col1.Style.Foreground
            .Should().Be(Color.Green);
    }

    [Fact]
    public void PrintTable_NotColoredColumns_TableIsWrittenWithOutColorToConsole()
    {
        // Arrange
        var ansiConsole = A.Fake<IAnsiConsole>();

        var items = new[]
        {
            new TestItem { Name = "Item1", Value = 10 }
        };

        var columns = new[]
        {
            new TableColumnDef<TestItem>(x => x.Name, "Name"),
            new TableColumnDef<TestItem>(x => x.Value, "Value")
        };

        Table? capturedTable = null;
        A.CallTo(() => ansiConsole.Write(A<IRenderable>.Ignored))
            .Invokes(call => capturedTable = call.Arguments.Get<Table>(0));

        // Act
        ansiConsole.PrintTable(items, columns);

        // Assert
        capturedTable.Should().NotBeNull();
        capturedTable!.Rows.Count.Should().Be(2);

        var row = (capturedTable.Rows as IReadOnlyList<TableRow>)[1];

        var col0 = row[0].GetSegments(AnsiConsole.Create(new AnsiConsoleSettings())).First();
        col0.Text
            .Should().Be("Item1");

        col0.Style.Foreground
            .Should().Be(Color.Default);

        var col1 = row[1].GetSegments(AnsiConsole.Create(new AnsiConsoleSettings())).First();
        col1.Text
            .Should().Be("10");

        col1.Style.Foreground
            .Should().Be(Color.Default);
    }

    private class TestItem
    {
        public string Name { get; init; } = string.Empty;

        public int Value { get; init; }
    }
}
