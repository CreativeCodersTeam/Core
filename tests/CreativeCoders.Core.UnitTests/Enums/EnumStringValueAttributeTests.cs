using CreativeCoders.Core.Enums;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Enums;

public class EnumStringValueAttributeTests
{
    [Fact]
    public void Ctor_CallWithNullText_ThrowsException()
    {
        _ = new EnumStringValueAttribute(null);
    }

    [Fact]
    public void Ctor_CallWithStringEmpty_Succeeds()
    {
        var attr = new EnumStringValueAttribute(string.Empty);

        Assert.Equal(string.Empty, attr.Text);
    }

    [Fact]
    public void Ctor_CallWithString_Succeeds()
    {
        var attr = new EnumStringValueAttribute("Text");

        Assert.Equal("Text", attr.Text);
    }
}
