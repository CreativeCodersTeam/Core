using System.Collections.Generic;
using CreativeCoders.SysConsole.CliArguments.Options;
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
