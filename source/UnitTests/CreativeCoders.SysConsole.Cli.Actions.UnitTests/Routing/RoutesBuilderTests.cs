using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using CreativeCoders.Core.Collections;
using CreativeCoders.SysConsole.Cli.Actions.Definition;
using CreativeCoders.SysConsole.Cli.Actions.Routing;
using CreativeCoders.SysConsole.Cli.Actions.UnitTests.TestData;
using FluentAssertions;
using Xunit;

namespace CreativeCoders.SysConsole.Cli.Actions.UnitTests.Routing;

public class RoutesBuilderTests
{
    [Fact]
    public void BuildRoutes_ForDemoController_TwoActionRoutesCreated()
    {
        var builder = new RoutesBuilder();

        builder.AddController(typeof(DemoCliController));

        // Act
        var routes = builder.BuildRoutes().ToArray();

        // Assert
        routes
            .Should()
            .HaveCount(2);

        routes.First().RouteParts
            .Should()
            .ContainInOrder("demo", "test");

        routes.First().ControllerType
            .Should()
            .Be(typeof(DemoCliController));

        routes.First().ActionMethod
            .Should()
            .BeSameAs(typeof(DemoCliController).GetMethod(nameof(DemoCliController.DoAsync)));

        routes.Last().RouteParts
            .Should()
            .ContainInOrder("demo", "more", "command");

        routes.Last().ControllerType
            .Should()
            .Be(typeof(DemoCliController));

        routes.Last().ActionMethod
            .Should()
            .BeSameAs(typeof(DemoCliController).GetMethod(nameof(DemoCliController.DoMoreAsync)));
    }

    [Fact]
    public void BuildRoutes_ForControllerClassWithoutControllerAttribute_ReturnsEmptyRoutes()
    {
        var builder = new RoutesBuilder();

        builder.AddController(typeof(TestNoneController));

        // Act
        var routes = builder.BuildRoutes();

        // Assert
        routes
            .Should()
            .BeEmpty();
    }

    [Fact]
    public void BuildRoutes_ControllerWithDefault_ReturnRoutesWithDefault()
    {
        var expectedRouteParts = new[]
        {
            Array.Empty<string>(),
            new[] {"help"},
            new[] {"setup"},
            new[] {"config"},
            new[] {"config", "help"},
            new[] {"config", "setup"}
        };

        var builder = new RoutesBuilder();

        builder.AddController(typeof(TestControllerWithDefault));

        // Act
        var routes = builder.BuildRoutes().ToArray();

        // Assert
        routes
            .Should()
            .HaveCount(expectedRouteParts.Length);

        var routeParts = routes.Select(x => x.RouteParts).ToArray();

        foreach (var expectedRoutePart in expectedRouteParts)
        {
            routeParts
                .Should()
                .ContainEquivalentOf(expectedRoutePart);
        }
    }

    private static void Print(string text)
    {
        Console.WriteLine(text);
    }
}

public class TestNoneController
{
    [CliAction]
    // ReSharper disable once UnusedMember.Global
    public Task<CliActionResult> TestAsync()
    {
        return Task.FromResult(new CliActionResult());
    }
}
