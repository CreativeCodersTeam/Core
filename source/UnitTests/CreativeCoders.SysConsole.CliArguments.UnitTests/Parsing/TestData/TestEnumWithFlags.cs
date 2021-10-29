using System;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.CliArguments.UnitTests.Parsing.TestData
{
    [PublicAPI]
    [Flags]
    public enum TestEnumWithFlags
    {
        Default,
        None,
        Ok,
        Failed,
        Custom,
        // ReSharper disable once InconsistentNaming
        OK
    }
}
