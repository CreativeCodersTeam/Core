using System.Text;
using CreativeCoders.Net.XmlRpc.Reader;
using CreativeCoders.Net.XmlRpc.Reader.Values;
using Xunit;

namespace CreativeCoders.Net.UnitTests.XmlRpc.Reader.Values;

public class ValueReadersTests
{
    [Fact]
    public void GetReader_ForString_ReturnsStringValueReader()
    {
        CheckReaders<StringValueReader>("string");
    }

    [Fact]
    public void GetReader_ForInteger_ReturnsIntegerValueReader()
    {
        CheckReaders<IntegerValueReader>("int");
        CheckReaders<IntegerValueReader>("i4");
    }

    [Fact]
    public void GetReader_ForDouble_ReturnsDoubleValueReader()
    {
        CheckReaders<DoubleValueReader>("double");
    }

    [Fact]
    public void GetReader_ForDateTime_ReturnsDateTimeValueReader()
    {
        CheckReaders<DateTimeValueReader>("dateTime.iso8601");
    }

    [Fact]
    public void GetReader_ForBase64_ReturnsBase64ValueReader()
    {
        CheckReaders<Base64ValueReader>("base64");
    }

    [Fact]
    public void GetReader_ForBoolean_ReturnsBooleanValueReader()
    {
        CheckReaders<BooleanValueReader>("boolean");
    }

    [Fact]
    public void GetReader_ForArray_ReturnsArrayValueReader()
    {
        CheckReaders<ArrayValueReader>("array");
    }

    [Fact]
    public void GetReader_ForStruct_ReturnsStructValueReader()
    {
        CheckReaders<StructValueReader>("struct");
    }

    private static void CheckReaders<T>(string dataType)
    {
        var readers = new ValueReaders(Encoding.UTF8) as IValueReaders;

        var reader = readers.GetReader(dataType);

        Assert.IsType<T>(reader);
    }
}
