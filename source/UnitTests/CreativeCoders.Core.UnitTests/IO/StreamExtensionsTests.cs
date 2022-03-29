using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CreativeCoders.Core.IO;
using Xunit;

#pragma warning disable 618

namespace CreativeCoders.Core.UnitTests.IO;
#pragma warning disable SYSLIB0001 // Type or member is obsolete
public class StreamExtensionsTests
{
    [Fact]
    public void ReadAsString_TextAsBytesInMemoryStream_ReturnsInputString()
    {
        var stream = new MemoryStream(Encoding.UTF8.GetBytes("Hello World"));

        Assert.Equal("Hello World", stream.ReadAsString());
    }

    [Fact]
    public void ReadAsString_Utf7TextAsBytesInMemoryStreamReadAsUtf8_NotReturnsInput()
    {
        var stream = new MemoryStream(Encoding.UTF7.GetBytes("Ä"));

        Assert.NotEqual("Ä", stream.ReadAsString(Encoding.UTF8));
    }

    [Fact]
    public void ReadAsString_Utf7TextAsBytesInMemoryStreamReadAsUtf7_ReturnsInput()
    {
        var stream = new MemoryStream(Encoding.UTF7.GetBytes("Ä"));

        Assert.Equal("Ä", stream.ReadAsString(Encoding.UTF7));
    }

    [Fact]
    public void ReadAsString_Utf8TextAsBytesInMemoryStreamReadAsUtf7WithAutodetectBOM_ReturnsInput()
    {
        var stream =
            new MemoryStream(new byte[] {239, 187, 191}.Concat(Encoding.UTF8.GetBytes("Ä")).ToArray());

        Assert.Equal("Ä", stream.ReadAsString(Encoding.UTF7, true));
    }

    [Fact]
    public async Task ReadAsStringAsync_TextAsBytesInMemoryStream_ReturnsInputString()
    {
        var stream = new MemoryStream(Encoding.UTF8.GetBytes("Hello World"));

        Assert.Equal("Hello World", await stream.ReadAsStringAsync());
    }

    [Fact]
    public async Task ReadAsStringAsync_Utf7TextAsBytesInMemoryStreamReadAsUtf8_NotReturnsInput()
    {
        var stream = new MemoryStream(Encoding.UTF7.GetBytes("Ä"));

        Assert.NotEqual("Ä", await stream.ReadAsStringAsync(Encoding.UTF8));
    }

    [Fact]
    public async Task ReadAsStringAsync_Utf7TextAsBytesInMemoryStreamReadAsUtf7_ReturnsInput()
    {
        var stream = new MemoryStream(Encoding.UTF7.GetBytes("Ä"));

        Assert.Equal("Ä", await stream.ReadAsStringAsync(Encoding.UTF7));
    }

    [Fact]
    public async Task
        ReadAsStringAsync_Utf8TextAsBytesInMemoryStreamReadAsUtf7WithAutodetectBOM_ReturnsInput()
    {
        var stream =
            new MemoryStream(new byte[] {239, 187, 191}.Concat(Encoding.UTF8.GetBytes("Ä")).ToArray());


        Assert.Equal("Ä", await stream.ReadAsStringAsync(Encoding.UTF7, true));
    }
}
#pragma warning restore SYSLIB0001 // Type or member is obsolete
#pragma warning restore 618
