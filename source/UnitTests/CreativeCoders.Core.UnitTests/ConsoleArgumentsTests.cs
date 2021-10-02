using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;

namespace CreativeCoders.Core.UnitTests
{
    public class ConsoleArgumentsTests
    {
        [Fact]
        [SuppressMessage("ReSharper", "UnusedVariable")]
        public void ConsoleArgumentsTestsCtor()
        {
            Assert.Throws<ArgumentNullException>(() => new ConsoleArguments(null, false));
            var consoleArgs0 = new ConsoleArguments(new[] {"test"}, false);
            var consoleArgs1 = new ConsoleArguments(new[] {"test"}, true);
            var consoleArgs2 = new ConsoleArguments(Array.Empty<string>(), false);
            var consoleArgs3 = new ConsoleArguments(Array.Empty<string>(), true);
        }

        [Fact]
        public void ConsoleArgumentsTestsCaseSensitive()
        {
            const string param0 = "test";
            const string param1 = "/verbose:on";
            const string param2 = "/recursive=yes";
            const string param3 = "value1";
            const string param4 = "/value2";

            var consoleArgs = new ConsoleArguments(new[] {param0, param1, param2, param3, param4}, true);

            Assert.Equal("on", consoleArgs["verbose"]);
            Assert.Empty(consoleArgs["Verbose"]);
            Assert.Equal("yes", consoleArgs["recursive"]);
            Assert.Empty(consoleArgs["Recursive"]);
            Assert.Equal(2, consoleArgs.Params.Count());
            Assert.Equal(3, consoleArgs.Values.Count());

            var param0Found = consoleArgs.Params.Contains(param0);
            var param3Found = consoleArgs.Params.Contains(param3);

            Assert.True(param0Found, "param0 not found");
            Assert.True(param3Found, "param3 not found");

            Assert.True(consoleArgs.IsSet(param0));
            Assert.False(consoleArgs.IsSet("test1"));
        }
    }
}
