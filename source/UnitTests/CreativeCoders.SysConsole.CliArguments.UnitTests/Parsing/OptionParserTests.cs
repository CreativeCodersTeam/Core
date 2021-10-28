using System;
using CreativeCoders.SysConsole.CliArguments.Exceptions;
using CreativeCoders.SysConsole.CliArguments.Options;
using CreativeCoders.SysConsole.CliArguments.Parsing;
using CreativeCoders.SysConsole.CliArguments.UnitTests.Parsing.TestData;
using FluentAssertions;
using Xunit;

namespace CreativeCoders.SysConsole.CliArguments.UnitTests.Parsing
{
    public class OptionParserTests
    {
        [Fact]
        public void Parse_ValueAndParameterWithLongName_PropertiesAreSetCorrect()
        {
            var args = new[] { "hello", "--text", "TestText" };

            var parser = new OptionParser();

            // Act
            var option = parser.Parse(typeof(TestOptionForParser), args) as TestOptionForParser;

            // Assert
            option
                .Should()
                .NotBeNull();

            option!.HelloWorld
                .Should()
                .Be("hello");

            option.TextValue
                .Should()
                .Be("TestText");
        }

        [Fact]
        public void Parse_ValueAndParameterWithShortName_PropertiesAreSetCorrect()
        {
            var args = new[] { "hello", "-t", "TestText" };

            var parser = new OptionParser();

            // Act
            var option = parser.Parse(typeof(TestOptionForParser), args) as TestOptionForParser;

            // Assert
            option
                .Should()
                .NotBeNull();

            option!.HelloWorld
                .Should()
                .Be("hello");

            option.TextValue
                .Should()
                .Be("TestText");
        }

        [Fact]
        public void Parse_IntValueAndParameterWithLongName_PropertiesAreSetCorrect()
        {
            const int expectedValue0 = 1234;
            const int expectedValue1 = 4321;

            var args = new[]
                {"hello", "--integer2", expectedValue1.ToString(), "--integer", expectedValue0.ToString()};

            var parser = new OptionParser();

            // Act
            var option = parser.Parse(typeof(TestOptionWithInt), args) as TestOptionWithInt;

            // Assert
            option
                .Should()
                .NotBeNull();

            option!.IntValue
                .Should()
                .Be(expectedValue0);

            option.IntValue2
                .Should()
                .Be(expectedValue1);
        }

        [Fact]
        public void Parse_IntValueAndParameterWithShortName_PropertiesAreSetCorrect()
        {
            const int expectedValue0 = 1234;
            const int expectedValue1 = 4321;

            var args = new[] { "hello", "-j", expectedValue1.ToString(), "-i", expectedValue0.ToString() };

            var parser = new OptionParser();

            // Act
            var option = parser.Parse(typeof(TestOptionWithInt), args) as TestOptionWithInt;

            // Assert
            option
                .Should()
                .NotBeNull();

            option!.IntValue
                .Should()
                .Be(expectedValue0);

            option.IntValue2
                .Should()
                .Be(expectedValue1);

            option.DefIntValue
                .Should()
                .Be(1357);
        }

        [Fact]
        public void Parse_RequiredParameterMissing_ThrowsException()
        {
            const int expectedValue1 = 4321;

            var args = new[] { "hello", "-j", expectedValue1.ToString() };

            var parser = new OptionParser();

            // Act
            Action act = () => parser.Parse(typeof(TestOptionWithInt), args);

            // Assert
            var exception = act
                .Should()
                .Throw<RequiredArgumentMissingException>()
                .Which;

            exception.MissingProperty
                .Should()
                .BeSameAs(typeof(TestOptionWithInt).GetProperty(nameof(TestOptionWithInt.IntValue)));

            exception.ParameterAttribute
                .Should()
                .BeEquivalentTo(new OptionParameterAttribute('i', "integer"){IsRequired = true});
        }

        [Fact]
        public void Parse_BoolValues_PropertiesAreSetCorrect()
        {
            var args = new[] { "-v", "--bold", "true" };

            var parser = new OptionParser();

            // Act
            var option = parser.Parse(typeof(TestOptionWithBool), args) as TestOptionWithBool;

            // Assert
            option
                .Should()
                .NotBeNull();

            option!.Verbose
                .Should()
                .BeTrue();

            option.Bold
                .Should()
                .BeTrue();
        }

        [Fact]
        public void Parse_BoolValuesInvalidFormat_PropertyIsSetFalse()
        {
            var args = new[] { "-v", "--bold", "1234" };

            var parser = new OptionParser();

            // Act
            var option = parser.Parse(typeof(TestOptionWithBool), args) as TestOptionWithBool;

            // Assert
            option
                .Should()
                .NotBeNull();

            option!.Verbose
                .Should()
                .BeTrue();

            option.Bold
                .Should()
                .BeFalse();
        }

        [Fact]
        public void Parse_RequiredValueMissing_ThrowsException()
        {
            var args = Array.Empty<string>();

            var parser = new OptionParser();

            // Act
            Action act = () => parser.Parse(typeof(TestOptionWithValueOption), args);

            // Assert
            var exception = act
                .Should()
                .Throw<RequiredArgumentMissingException>()
                .Which;

            exception.MissingProperty
                .Should()
                .BeSameAs(typeof(TestOptionWithValueOption).GetProperty(nameof(TestOptionWithValueOption.TestValue)));

            exception.ValueAttribute
                .Should()
                .BeEquivalentTo(new OptionValueAttribute(0) { IsRequired = true });
        }

        [Fact]
        public void Parse_ValueDefaultValue_DefaultValueIsSet()
        {
            var args = Array.Empty<string>();

            var parser = new OptionParser();

            // Act
            var options = parser.Parse(typeof(TestOptionWithTwoValues), args) as TestOptionWithTwoValues;

            // Assert
            options
                .Should()
                .NotBeNull();

            options!.FirstValue
                .Should()
                .Be(null);

            options.SecondValue
                .Should()
                .Be("Fallback");
        }

        [Fact]
        public void Parse_OptionCtorWithArgs_ThrowsException()
        {
            var args = Array.Empty<string>();

            var parser = new OptionParser();

            // Act
            Action act = () => parser.Parse(typeof(TestOptionWithInvalidCtor), args);

            // Assert
            act
                .Should()
                .Throw<OptionCreationFailedException>()
                .Which
                .OptionType
                .Should()
                .Be(typeof(TestOptionWithInvalidCtor));
        }

        [Fact]
        public void Parse_OptionNull_ThrowsException()
        {
            var args = Array.Empty<string>();

            var parser = new OptionParser();

            // Act
            Action act = () => parser.Parse(typeof(int?), args);

            // Assert
            act
                .Should()
                .Throw<OptionCreationFailedException>()
                .Which
                .OptionType
                .Should()
                .Be(typeof(int?));
        }

        [Theory]
        [InlineData("Ok", TestEnum.Ok)]
        [InlineData("OK", TestEnum.OK)]
        [InlineData("Default", TestEnum.Default)]
        [InlineData("default", TestEnum.Default)]
        [InlineData("Custom", TestEnum.Custom)]
        [InlineData("CUSTOM", TestEnum.Custom)]
        [InlineData("Failed", TestEnum.Failed)]
        [InlineData("failED", TestEnum.Failed)]
        [InlineData("None", TestEnum.None)]
        [InlineData("nONe", TestEnum.None)]
        public void Parse_EnumValue_PropertyIsSetCorrect(string argValue, TestEnum enumValue)
        {
            var args = new[] { "-e", argValue };

            var parser = new OptionParser();

            // Act
            var option = parser.Parse(typeof(TestOptionWithEnum), args) as TestOptionWithEnum;

            // Assert
            option
                .Should()
                .NotBeNull();

            option!.EnumValue
                .Should()
                .Be(enumValue);
        }

        [Theory]
        [InlineData("Hello", "World")]
        [InlineData("World", "Hello")]
        [InlineData("Abc", "ABC")]
        [InlineData("qwertz", "QWERTZ")]
        public void Parse_PropertyWithConverter_PropertyIsSetCorrect(string argValue, string propertyValue)
        {
            var args = new[] { "-t", argValue };

            var parser = new OptionParser();

            // Act
            var option = parser.Parse(typeof(TestOptionWithConverter), args) as TestOptionWithConverter;

            // Assert
            option
                .Should()
                .NotBeNull();

            option!.Text
                .Should()
                .Be(propertyValue);
        }
    }
}
