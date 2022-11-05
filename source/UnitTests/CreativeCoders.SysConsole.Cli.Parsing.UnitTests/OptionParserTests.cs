using System;
using System.Collections.Generic;
using CreativeCoders.SysConsole.Cli.Parsing.Exceptions;
using CreativeCoders.SysConsole.Cli.Parsing.UnitTests.TestData;
using FluentAssertions;
using Xunit;

namespace CreativeCoders.SysConsole.Cli.Parsing.UnitTests;

public class OptionParserTests
{
    [Fact]
    public void Parse_ValueAndParameterWithLongName_PropertiesAreSetCorrect()
    {
        var args = new[] {"hello", "--text", "TestText"};

        var parser = new OptionParser(typeof(TestOptionForParser));

        // Act
        var option = parser.Parse(args) as TestOptionForParser;

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
        var args = new[] {"hello", "-t", "TestText"};

        var parser = new OptionParser(typeof(TestOptionForParser));

        // Act
        var option = parser.Parse(args) as TestOptionForParser;

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

        var parser = new OptionParser(typeof(TestOptionWithInt));

        // Act
        var option = parser.Parse(args) as TestOptionWithInt;

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

        var args = new[] {"hello", "-j", expectedValue1.ToString(), "-i", expectedValue0.ToString()};

        var parser = new OptionParser(typeof(TestOptionWithInt));

        // Act
        var option = parser.Parse(args) as TestOptionWithInt;

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

        var args = new[] {"hello", "-j", expectedValue1.ToString()};

        var parser = new OptionParser(typeof(TestOptionWithInt));

        // Act
        Action act = () => parser.Parse(args);

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
            .BeEquivalentTo(new OptionParameterAttribute('i', "integer") {IsRequired = true});
    }

    [Fact]
    public void Parse_BoolValues_PropertiesAreSetCorrect()
    {
        var args = new[] {"-v", "--bold", "true"};

        var parser = new OptionParser(typeof(TestOptionWithBool));

        // Act
        var option = parser.Parse(args) as TestOptionWithBool;

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
        var args = new[] {"-v", "--bold", "1234"};

        var parser = new OptionParser(typeof(TestOptionWithBool));

        // Act
        var option = parser.Parse(args) as TestOptionWithBool;

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

        var parser = new OptionParser(typeof(TestOptionWithValueOption));

        // Act
        Action act = () => parser.Parse(args);

        // Assert
        var exception = act
            .Should()
            .Throw<RequiredArgumentMissingException>()
            .Which;

        exception.MissingProperty
            .Should()
            .BeSameAs(typeof(TestOptionWithValueOption).GetProperty(
                nameof(TestOptionWithValueOption.TestValue)));

        exception.ValueAttribute
            .Should()
            .BeEquivalentTo(new OptionValueAttribute(0) {IsRequired = true});
    }

    [Fact]
    public void Parse_ValueDefaultValue_DefaultValueIsSet()
    {
        var args = Array.Empty<string>();

        var parser = new OptionParser(typeof(TestOptionWithTwoValues));

        // Act
        var options = parser.Parse(args) as TestOptionWithTwoValues;

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

        var parser = new OptionParser(typeof(TestOptionWithInvalidCtor));

        // Act
        Action act = () => parser.Parse(args);

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

        var parser = new OptionParser(typeof(int?));

        // Act
        Action act = () => parser.Parse(args);

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
    [InlineData("ok", TestEnum.Default)]
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
        var args = new[] {"-e", argValue};

        var parser = new OptionParser(typeof(TestOptionWithEnum));

        // Act
        var option = parser.Parse(args) as TestOptionWithEnum;

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
        var args = new[] {"-t", argValue};

        var parser = new OptionParser(typeof(TestOptionWithConverter));

        // Act
        var option = parser.Parse(args) as TestOptionWithConverter;

        // Assert
        option
            .Should()
            .NotBeNull();

        option!.Text
            .Should()
            .Be(propertyValue);
    }

    [Theory]
    [InlineData("1,2,3,4,5", new[] {1, 2, 3, 4, 5})]
    [InlineData("1", new[] {1})]
    public void Parse_PropertyIsIEnumerableOfInt_PropertyIsSetCorrect(string argValue,
        IEnumerable<int> intValues)
    {
        var args = new[] {"-i", argValue};

        var parser = new OptionParser(typeof(TestOptionWithIntEnumerable));

        // Act
        var option =
            parser.Parse(args) as TestOptionWithIntEnumerable;

        // Assert
        option
            .Should()
            .NotBeNull();

        option!.IntValues
            .Should()
            .ContainInOrder(intValues);
    }

    [Theory]
    [InlineData("Failed,Ok", TestEnumWithFlags.Ok | TestEnumWithFlags.Failed)]
    [InlineData("Ok", TestEnumWithFlags.Ok)]
    [InlineData("None,Custom,Ok", TestEnumWithFlags.Ok | TestEnumWithFlags.Custom | TestEnumWithFlags.None)]
    public void Parse_PropertyIsEnumWithFlags_EnumFlagsAreSetCorrect(string argValue,
        TestEnumWithFlags enumWithFlags)
    {
        var args = new[] {"-e", argValue};

        var parser = new OptionParser(typeof(TestOptionWithEnumFlags));

        // Act
        var option = parser.Parse(args) as TestOptionWithEnumFlags;

        // Assert
        option
            .Should()
            .NotBeNull();

        option!.EnumValue
            .Should()
            .Be(enumWithFlags);
    }

    [Theory]
    [InlineData("test")]
    [InlineData("test", "-f", "value0")]
    [InlineData("-f", "value0", "test")]
    [InlineData("test", "-f", "value0", "--second", "value1")]
    [InlineData("--second", "value1", "-f", "value0", "test")]
    public void Parse_NotAllArgsMatch_ThrowsException(params string[] args)
    {
        var parser = new OptionParser(typeof(TestOptionAllArgsMustMatch));

        // Act
        Action act = () => parser.Parse(args);

        // Assert
        act
            .Should()
            .Throw<NotAllArgumentsMatchException>()
            .Which
            .NotMatchedArgs
            .Should()
            .HaveCount(1)
            .And
            .BeEquivalentTo(new[] {new OptionArgument {Kind = OptionArgumentKind.Value, Value = "test"}});
    }

    [Theory]
    [InlineData(typeof(string[]))]
    [InlineData(typeof(IEnumerable<string>))]
    public void Parse_ParameterTypeIsArrayOfString_ArgsAreReturned(Type optionType)
    {
        var parser = new OptionParser(optionType);

        var args = new[] {"first", "second", "some", "more"};

        // Act
        var option = parser.Parse(args) as string[];

        // Assert
        option
            .Should()
            .Equal(args);
    }

    [Theory]
    [InlineData(true, "-c")]
    [InlineData(true, "-c", "true")]
    [InlineData(false, "-c", "false")]
    [InlineData(false)]
    public void Test(bool expectedValue, params string[] args)
    {
        var parser = new OptionParser(typeof(TestOptionWithSingleBool));

        // Act
        var option = (TestOptionWithSingleBool) parser.Parse(args);

        // Assert
        option.CreateData
            .Should()
            .Be(expectedValue);
    }
}
