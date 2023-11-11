using System;
using System.Diagnostics.CodeAnalysis;

namespace CreativeCoders.Core.UnitTests.Enums;

[Flags]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public enum TestEnumWithFlags
{
    Test1 = 1,
    Ok = 2,
    Test2 = 4,
    Item = 8
}
