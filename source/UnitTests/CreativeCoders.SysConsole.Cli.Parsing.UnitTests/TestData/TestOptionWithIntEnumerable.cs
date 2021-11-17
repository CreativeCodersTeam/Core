using System.Collections.Generic;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.Cli.Parsing.UnitTests.TestData
{
    [PublicAPI]
    public class TestOptionWithIntEnumerable
    {
        [OptionParameter('i', "integers")]
        public IEnumerable<int>? IntValues { get; set; }
    }
}
