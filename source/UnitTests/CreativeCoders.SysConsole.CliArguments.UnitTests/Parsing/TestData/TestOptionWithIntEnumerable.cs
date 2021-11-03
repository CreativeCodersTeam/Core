using System.Collections.Generic;
using CreativeCoders.SysConsole.Cli.Parsing;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.CliArguments.UnitTests.Parsing.TestData
{
    [PublicAPI]
    public class TestOptionWithIntEnumerable
    {
        [OptionParameter('i', "integers")]
        public IEnumerable<int>? IntValues { get; set; }
    }
}
