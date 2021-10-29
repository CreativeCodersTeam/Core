using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CreativeCoders.SysConsole.CliArguments.Options;
using CreativeCoders.SysConsole.CliArguments.Parsing.Properties;
using CreativeCoders.SysConsole.CliArguments.Parsing.Properties.ValueConverters;
using FluentAssertions;
using Xunit;

namespace CreativeCoders.SysConsole.CliArguments.UnitTests.Parsing.Properties.ValueConverters
{
    public class EnumerableValueConverterTests
    {
        [Fact]
        public void Convert_ValueIsNull_ReturnsDoNothing()
        {
            var converter = new EnumerableValueConverter();

            // Act
            var result = converter.Convert(null, typeof(IEnumerable),
                new OptionParameterAttribute('v', "values"));

            // Assert
            result
                .Should()
                .Be(ConverterAction.DoNothing);
        }

        [Fact]
        public void Convert_TargetTypeIsNotIEnumerable_ReturnsDoNothing()
        {
            var converter = new EnumerableValueConverter();

            // Act
            var result = converter.Convert("1,2,3,4,5", typeof(object),
                new OptionParameterAttribute('v', "values"));

            // Assert
            result
                .Should()
                .Be(ConverterAction.DoNothing);
        }

        [Fact]
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "<Pending>")]
        public void Convert_CommaSeparatedIntegers_ReturnsIntegers()
        {
            const string arg = "1,2,3,4,5";

            var converter = new EnumerableValueConverter();

            // Act
            var result = converter.Convert(arg, typeof(IEnumerable<int>),
                new OptionParameterAttribute('t', "test") {Separator = ','}) as IEnumerable<int>;

            // Assert
            result
                .Should()
                .NotBeNull();

            result
                .Should()
                .HaveCount(5);

            result
                .Should()
                .ContainInOrder(new[] {1, 2, 3, 4, 5});
        }
    }
}
