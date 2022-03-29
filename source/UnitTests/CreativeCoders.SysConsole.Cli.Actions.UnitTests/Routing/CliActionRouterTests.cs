using System;
using System.Collections.Generic;
using System.Linq;
using CreativeCoders.SysConsole.Cli.Actions.Routing;
using CreativeCoders.SysConsole.Cli.Actions.UnitTests.TestData;
using FluentAssertions;
using Xunit;

namespace CreativeCoders.SysConsole.Cli.Actions.UnitTests.Routing;

public class CliActionRouterTests
{
    [Theory]
    [InlineData(new[] {"demo", "command"}, new[] {"demo", "command"}, new string[0])]
    [InlineData(new[] {"demo", "command", "test1"}, new[] {"demo", "command"}, new[] {"test1"})]
    [InlineData(new[] {"demo", "command", "test2"}, new[] {"demo"}, new[] {"command", "test2"})]
    public void FindRoute_RouteIsMatched_ReturnsRoute(string[] arguments, string[] routeParts,
        string[] expectedArgs)
    {
        var args = arguments.ToList();

        var route0 = CreateActionRoute<DemoCliController>(nameof(DemoCliController.DoAsync), routeParts);

        var router = new CliActionRouter() as ICliActionRouter;

        router.AddRoute(route0);

        // Act
        var result = router.FindRoute(args);

        // Assert
        result
            .Should()
            .Be(route0);

        args
            .Should()
            .HaveCount(expectedArgs.Length)
            .And
            .ContainInOrder(expectedArgs);
    }

    [Fact]
    public void FindRoute_RouteWithEmptyActionRoute_ReturnsDefaultDemoRoute()
    {
        var args = new List<string> {"demo"};

        var route0 =
            CreateActionRoute<DefaultCliController>(nameof(DefaultCliController.DoDefaultAsync),
                "demo");

        var route1 =
            CreateActionRoute<DefaultCliController>(nameof(DefaultCliController.DoCommandAsync),
                "command");

        var router = new CliActionRouter() as ICliActionRouter;

        router.AddRoute(route1);
        router.AddRoute(route0);

        // Act
        var foundRoute = router.FindRoute(args);

        // Assert
        foundRoute
            .Should()
            .BeSameAs(route0);

        args
            .Should()
            .BeEmpty();

        router.ActionRoutes
            .Should()
            .HaveCount(2)
            .And
            .ContainInOrder(route1, route0);
    }

    [Fact]
    public void FindRoute_RouteActionRouteInDefaultController_ReturnsDefaultDemoRoute()
    {
        var args = new List<string> {"command"};

        var route0 = CreateActionRoute<DefaultCliController>(nameof(DefaultCliController.DoDefaultAsync),
            "", "command");

        var router = new CliActionRouter() as ICliActionRouter;

        router.AddRoute(route0);

        // Act
        var foundRoute = router.FindRoute(args);

        // Assert
        foundRoute
            .Should()
            .BeSameAs(route0);
    }

    [Fact]
    public void FindRoute_DefaultAction_ReturnsDefaultDemoRoute()
    {
        var args = new List<string> {"option"};

        var route0 = CreateActionRoute<DefaultCliController>(nameof(DefaultCliController.DoDefaultAsync),
            "", "");

        var router = new CliActionRouter() as ICliActionRouter;

        router.AddRoute(route0);

        // Act
        var foundRoute = router.FindRoute(args);

        // Assert
        foundRoute
            .Should()
            .BeSameAs(route0);
    }

    [Fact]
    public void FindRoute()
    {
        var args = new List<string> {"controller", "action"};

        var route0 = CreateActionRoute<DefaultCliController>(nameof(DefaultCliController.DoDefaultAsync),
            "controller");

        var route1 = CreateActionRoute<DefaultCliController>(nameof(DefaultCliController.DoCommandAsync),
            "controller", "action");

        var router = new CliActionRouter() as ICliActionRouter;

        router.AddRoute(route0);
        router.AddRoute(route1);

        // Act
        var foundRoute = router.FindRoute(args);

        // Assert
        foundRoute
            .Should()
            .BeSameAs(route1);
    }

    private static CliActionRoute CreateActionRoute<TController>(string methodName,
        params string[] routeParts)
    {
        return new CliActionRoute(
            typeof(TController),
            typeof(TController).GetMethod(methodName)
            ?? throw new InvalidOperationException(),
            routeParts);
    }
}
