using CreativeCoders.SysConsole.Cli.Actions.Routing;
using CreativeCoders.SysConsole.Cli.Actions.UnitTests.TestData;
using FluentAssertions;
using Xunit;

namespace CreativeCoders.SysConsole.Cli.Actions.UnitTests.Routing
{
    public class CliActionRouterTests
    {
        [Theory]
        [InlineData(new[] { "demo", "command" }, new []{ "demo", "command", "test1" })]
        [InlineData(new[] { "demo" }, new[] { "demo", "command", "test1" })]
        public void FindRoute_RouteIsMatched_ReturnsRoute(string[] routeParts, string[] args)
        {
            var route0 = CreateActionRoute<DemoCliController>(nameof(DemoCliController.DoAsync), routeParts);

            var router = new CliActionRouter();

            router.AddRoute(route0);

            // Act
            var result = router.FindRoute(args);

            // Assert
            result
                .Should()
                .Be(route0);
        }

        [Fact]
        public void FindRoute_RouteWithEmptyActionRoute_ReturnsDefaultDemoRoute()
        {
            var args = new[] {"demo"};

            var route0 =
                CreateActionRoute<DefaultCliController>(nameof(DefaultCliController.DoDefaultAsync),
                    "demo", "");

            var router = new CliActionRouter();

            router.AddRoute(route0);

            // Act
            var foundRoute = router.FindRoute(args);

            // Assert
            foundRoute
                .Should()
                .BeSameAs(route0);
        }

        [Fact]
        public void FindRoute_RouteActionRouteInDefaultController_ReturnsDefaultDemoRoute()
        {
            var args = new[] { "command" };

            var route0 = CreateActionRoute<DefaultCliController>(nameof(DefaultCliController.DoDefaultAsync),
                "", "command");

            var router = new CliActionRouter();

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
            var args = new[] { "option" };

            var route0 = CreateActionRoute<DefaultCliController>(nameof(DefaultCliController.DoDefaultAsync),
                "", "");

            var router = new CliActionRouter();

            router.AddRoute(route0);

            // Act
            var foundRoute = router.FindRoute(args);

            // Assert
            foundRoute
                .Should()
                .BeSameAs(route0);
        }

        private static CliActionRoute CreateActionRoute<TController>(string methodName, params string[] routeParts)
        {
            return new CliActionRoute(typeof(TController), typeof(TController).GetMethod(methodName),
                routeParts);
        }
    }
}
