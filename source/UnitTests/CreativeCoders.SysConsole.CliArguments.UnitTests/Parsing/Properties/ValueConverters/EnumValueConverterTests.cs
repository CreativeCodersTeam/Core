using CreativeCoders.SysConsole.CliArguments.Options;
using CreativeCoders.SysConsole.CliArguments.Parsing.Properties;
using CreativeCoders.SysConsole.CliArguments.Parsing.Properties.ValueConverters;
using FluentAssertions;
using Xunit;

namespace CreativeCoders.SysConsole.CliArguments.UnitTests.Parsing.Properties.ValueConverters
{
    public class EnumValueConverterTests
    {
        [Fact]
        public void Convert_TargetIsNotEnum_ReturnsDoNothing()
        {
            var converter = new EnumValueConverter();

            // Act
            var result = converter.Convert("Test", typeof(int),
                new OptionParameterAttribute('t', "test"));

            // Assert
            result
                .Should()
                .Be(ConverterAction.DoNothing);
        }
    }
}
