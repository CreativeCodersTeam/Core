using CreativeCoders.Core.Enums;

namespace CreativeCoders.Core.UnitTests.Enums;

[EnumStringValue("DefaultText")]
public enum TestEnum
{
    [EnumStringValue("none value")]
    None,
    [EnumStringValue("first entry")]
    FirstEntry,
    [EnumStringValue("extra info")]
    Extra,
    [EnumStringValue("some more")]
    SomeMore,
    ElementWithoutAttribute
}