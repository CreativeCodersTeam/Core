using CreativeCoders.Validation;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Validation;

public class ValidationFaultTests
{
    [Fact]
    public void Ctor_WithMessage_MessagePropIsSetCorrect()
    {
        var fault = new ValidationFault("Test");

        Assert.Equal("Test", fault.Message);
        Assert.Equal(string.Empty, fault.PropertyName);
    }

    [Fact]
    public void Ctor_WithMessageAndPropertyName_MessageAndPropertyNamePropIsSetCorrect()
    {
        var fault = new ValidationFault("TestProp", "Test");

        Assert.Equal("Test", fault.Message);
        Assert.Equal("TestProp", fault.PropertyName);
    }
}
