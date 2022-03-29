using CreativeCoders.Net.XmlRpc.Model.Values;
using CreativeCoders.Net.XmlRpc.Writer.Values;
using Xunit;

namespace CreativeCoders.Net.UnitTests.XmlRpc.Writer.Values;

public class ValueWritersTests
{
    [Fact]
    public void GetReader_ForString_ReturnsStringValueReader()
    {
        CheckWriters<StringValueWriter, StringValue>();
    }

    [Fact]
    public void GetReader_ForInteger_ReturnsIntegerValueReader()
    {
        CheckWriters<IntegerValueWriter, IntegerValue>();
    }

    [Fact]
    public void GetReader_ForDouble_ReturnsDoubleValueReader()
    {
        CheckWriters<DoubleValueWriter, DoubleValue>();
    }

    [Fact]
    public void GetReader_ForDateTime_ReturnsDateTimeValueReader()
    {
        CheckWriters<DateTimeValueWriter, DateTimeValue>();
    }

    [Fact]
    public void GetReader_ForBase64_ReturnsBase64ValueReader()
    {
        CheckWriters<Base64ValueWriter, Base64Value>();
    }

    [Fact]
    public void GetReader_ForBoolean_ReturnsBooleanValueReader()
    {
        CheckWriters<BooleanValueWriter, BooleanValue>();
    }

    [Fact]
    public void GetReader_ForArray_ReturnsArrayValueReader()
    {
        CheckWriters<ArrayValueWriter, ArrayValue>();
    }

    [Fact]
    public void GetReader_ForStruct_ReturnsStructValueReader()
    {
        CheckWriters<StructValueWriter, StructValue>();
    }

    private static void CheckWriters<TWriter, TValue>()
    {
        var writers = new ValueWriters();

        var writer = writers.GetWriter(typeof(TValue));

        Assert.IsType<TWriter>(writer);
    }
}