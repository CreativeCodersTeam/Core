using CreativeCoders.SysConsole.Cli.Parsing.Properties;
using CreativeCoders.SysConsole.Cli.Parsing.Properties.ValueConverters;
using FluentAssertions;
using Xunit;

namespace CreativeCoders.SysConsole.Cli.Parsing.UnitTests.Properties.ValueConverters
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
