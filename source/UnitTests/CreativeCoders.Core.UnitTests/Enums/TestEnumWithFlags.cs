using System;

namespace CreativeCoders.Core.UnitTests.Enums;

[Flags]
public enum TestEnumWithFlags
{
    Test1 = 1,
    Ok = 2,
    Test2 = 4,
    Item = 8
}
