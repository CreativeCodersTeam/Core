using CreativeCoders.Net.XmlRpc.Definition;
using Xunit;

namespace CreativeCoders.Net.UnitTests.XmlRpc.Definition;

public class XmlRpcMethodAttributeTests
{
    [Fact]
    public void Ctor_NoName_NameIsEmpty()
    {
        var attribute = new XmlRpcMethodAttribute();

        Assert.Empty(attribute.MethodName);
    }

    [Fact]
    public void Ctor_Name_NameIsSet()
    {
        var attribute = new XmlRpcMethodAttribute("Test1234");

        Assert.Equal("Test1234", attribute.MethodName);
    }
}
