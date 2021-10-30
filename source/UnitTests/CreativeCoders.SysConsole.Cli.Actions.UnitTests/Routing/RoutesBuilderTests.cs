using System.Linq;
using CreativeCoders.SysConsole.Cli.Actions.Routing;
using CreativeCoders.SysConsole.Cli.Actions.UnitTests.TestData;
using FluentAssertions;
using Xunit;

namespace CreativeCoders.SysConsole.Cli.Actions.UnitTests.Routing
{
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
    }
}
