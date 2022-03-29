using System;
using System.Collections.Generic;
using CreativeCoders.SysConsole.Cli.Actions.Help;
using CreativeCoders.SysConsole.Cli.Actions.Routing;
using CreativeCoders.SysConsole.Cli.Actions.UnitTests.TestData;
using CreativeCoders.SysConsole.Cli.Parsing.Help;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace CreativeCoders.SysConsole.Cli.Actions.UnitTests.Help;

public class CliActionHelpGeneratorTests
{
    [Fact]
    public void CreateHelp_GivenActionViaRouter_HelpTextAndOptionsHelpCorrect()
    {
        var expectedOptionsHelp = new OptionsHelp(
            Array.Empty<HelpEntry>(),
            Array.Empty<HelpEntry>());

        var optionsHelpGenerator = A.Fake<IOptionsHelpGenerator>();

        A.CallTo(() => optionsHelpGenerator.CreateHelp(typeof(OptionsForHelp)))
            .Returns(expectedOptionsHelp);

        var actionRouter = A.Fake<ICliActionRouter>();

        A
            .CallTo(() => actionRouter.FindRoute(A<IList<string>>.Ignored))
            .Returns(
                new CliActionRoute(
                    typeof(TestControllerWithDefault),
                    typeof(TestControllerWithDefault).GetMethod(nameof(TestControllerWithDefault.Setup))
                    ?? throw new InvalidOperationException(),
                    Array.Empty<string>()));

        var helpGenerator = new CliActionHelpGenerator(optionsHelpGenerator, actionRouter);

        // Act
        var help = helpGenerator.CreateHelp(new[] {"test"});

        // Assert
        help.HelpText
            .Should()
            .Be("Setups the config");

        help.OptionsHelp
            .Should()
            .Be(expectedOptionsHelp);

        A.CallTo(() => optionsHelpGenerator.CreateHelp(typeof(OptionsForHelp)))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void Test() { }
}
