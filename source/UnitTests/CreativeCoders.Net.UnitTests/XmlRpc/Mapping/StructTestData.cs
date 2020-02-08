using CreativeCoders.Net.XmlRpc.Definition;
using CreativeCoders.Net.XmlRpc.Definition.MemberConverters;

namespace CreativeCoders.Net.UnitTests.XmlRpc.Mapping
{
    public class StructTestData
    {
        [XmlRpcStructMember("IntTest")]
        public int IntValue { get; set; }

        [XmlRpcStructMember("StringTest")]
        public string StringValue { get; set; }

        [XmlRpcStructMember]
        public SubTestData SubData { get; set; }
    }

    public class StructTestDataWithArray : StructTestData
    {
        [XmlRpcStructMember]
        public SubTestData[] SubItems { get; set; }
    }

    public class SubTestData
    {
        [XmlRpcStructMember]
        public string Name { get; set; }

        [XmlRpcStructMember]
        public int Id { get; set; }
    }

    public class StructTestDataWithConverter
    {
        [XmlRpcStructMember(Converter = typeof(EnumMemberValueConverter<TestEnum>))]
        public TestEnum TestValue { get; set; }
    }

    public class StructTestDataWithBoolInt
    {
        [XmlRpcStructMember(Converter = typeof(BoolToIntegerValueMemberValueConverter))]
        public bool TestBoolValue { get; set; }
    }

    public class StructTestDataWithObjectValue
    {
        [XmlRpcStructMember]
        public object Value { get; set; }
    }

    public enum TestEnum
    {
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4
    }
}