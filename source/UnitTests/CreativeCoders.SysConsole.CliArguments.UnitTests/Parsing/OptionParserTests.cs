using CreativeCoders.SysConsole.CliArguments.Options;
using CreativeCoders.SysConsole.CliArguments.Parsing;
using FluentAssertions;
using Xunit;

namespace CreativeCoders.SysConsole.CliArguments.UnitTests.Parsing
{
    public class OptionParserTests
    {
        [Fact]
        public void Test()
        {
            var args = new[] { "hello", "--text", "TestText" };

            var parser = new OptionParser();

            var option = parser.Parse(typeof(TestOptionForParser), args) as TestOptionForParser;

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
    }

    public class TestOptionForParser
    {
        [OptionValue(0)]
        public string HelloWorld { get; set; }

        [OptionParameter('t', "text")]
        public string TextValue { get; set; }
    }
}
