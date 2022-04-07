using CreativeCoders.Net.XmlRpc.Definition;
using CreativeCoders.Net.XmlRpc.Definition.MemberConverters;
using JetBrains.Annotations;

namespace CreativeCoders.Net.UnitTests.XmlRpc.Mapping;

public class StructTestData
{
    [XmlRpcStructMember("IntTest")] public int IntValue { get; init; }

    [XmlRpcStructMember("StringTest")] public string StringValue { get; init; }

    [XmlRpcStructMember] public SubTestData SubData { get; init; }
}

public class StructTestDataWithArray : StructTestData
{
    [XmlRpcStructMember] public SubTestData[] SubItems { get; init; }
}

public class SubTestData
{
    [XmlRpcStructMember] public string Name { get; init; }

    [XmlRpcStructMember] public int Id { get; init; }
}

public class StructTestDataWithConverter
{
    [XmlRpcStructMember(Converter = typeof(EnumMemberValueConverter<TestEnum>))]
    public TestEnum TestValue { get; init; }
}

public class StructTestDataWithBoolInt
{
    [XmlRpcStructMember(Converter = typeof(BoolToIntegerValueMemberValueConverter))]
    public bool TestBoolValue { get; set; }
}

public class StructTestDataWithObjectValue
{
    [XmlRpcStructMember] public object Value { get; [UsedImplicitly] set; }
}

public enum TestEnum
{
    One = 1,
    Two = 2,
    Three = 3,
    Four = 4
}
