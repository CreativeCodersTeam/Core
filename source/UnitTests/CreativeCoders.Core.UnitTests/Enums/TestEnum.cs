using CreativeCoders.Core.Enums;

namespace CreativeCoders.Core.UnitTests.Enums
{
    public enum TestEnum
    {
        [EnumStringValue("none value")]
        None,
        [EnumStringValue("first entry")]
        FirstEntry,
        [EnumStringValue("extra info")]
        Extra,
        [EnumStringValue("some more")]
        SomeMore
    }

    public enum TestEnumWithInt
    {
        None = 0,
        Ok = 1,
        Test = 2
    }
}