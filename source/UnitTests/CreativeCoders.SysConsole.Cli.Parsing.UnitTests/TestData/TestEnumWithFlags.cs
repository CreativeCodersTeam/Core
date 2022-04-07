using System;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.Cli.Parsing.UnitTests.TestData;

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
