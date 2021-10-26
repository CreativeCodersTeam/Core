using System;
using CreativeCoders.SysConsole.CliArguments.Exceptions;
using CreativeCoders.SysConsole.CliArguments.Options;
using CreativeCoders.SysConsole.CliArguments.Parsing;
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
            act
                .Should()
                .Throw<RequiredArgumentMissingException>();
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
    }

    public class TestOptionForParser
    {
        [OptionValue(0)]
        public string HelloWorld { get; set; }

        [OptionParameter('t', "text")]
        public string TextValue { get; set; }
    }

    public class TestOptionWithInt
    {
        [OptionParameter('i', "integer", IsRequired = true)]
        public int IntValue { get; set; }

        [OptionParameter('j', "integer2")]
        public int IntValue2 { get; set; }

        [OptionParameter('d', "default", DefaultValue = 1357)]
        public int DefIntValue { get; set; }
    }

    public class TestOptionWithBool
    {
        [OptionParameter('v', "verbose")]
        public bool Verbose { get; set; }

        [OptionParameter('b', "bold")]
        public bool Bold { get; set; }
    }
}
