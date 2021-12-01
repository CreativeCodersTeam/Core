using System.Linq;
using CreativeCoders.SysConsole.Cli.Parsing.Help;
using CreativeCoders.SysConsole.Cli.Parsing.UnitTests.TestData;
using FluentAssertions;
using Xunit;

namespace CreativeCoders.SysConsole.Cli.Parsing.UnitTests.Help
{
    public class OptionsHelpGeneratorTests
    {
        [Fact]
        public void CreateHelp_OptionWithParameters_ParametersHelpIsCreatedCorrect()
        {
            var helpGenerator = new OptionsHelpGenerator() as IOptionsHelpGenerator;

            // Act
            var help = helpGenerator.CreateHelp(typeof(TestOptionForHelp));

            // Assert
            help.ParameterHelpEntries
                .Should()
                .HaveCount(2);

            var firstHelpParameter = help.ParameterHelpEntries.First();

            firstHelpParameter.ArgumentName
                .Should()
                .Be($"-{TestOptionForHelp.TitleShortName} --{TestOptionForHelp.TitleLongName} <{TestOptionForHelp.TitleName.ToUpper()}>");

            firstHelpParameter.HelpText
                .Should()
                .Be(TestOptionForHelp.TitleHelpText);
        }
    }
}
