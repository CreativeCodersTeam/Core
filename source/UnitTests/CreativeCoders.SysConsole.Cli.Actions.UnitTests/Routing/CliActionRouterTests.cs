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
            var route0 = new CliActionRoute(typeof(DemoCliController),
                typeof(DemoCliController).GetMethod(nameof(DemoCliController.DoAsync)),
                routeParts);

            var router = new CliActionRouter();

            router.AddRoute(route0);

            // Act
            var result = router.FindRoute(args);

            // Assert
            result
                .Should()
                .Be(route0);
        }
    }
}
