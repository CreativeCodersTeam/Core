using CreativeCoders.Core.ObjectLinking;
using Xunit;

namespace CreativeCoders.Core.UnitTests.ObjectLinking;

public class ConverterTests
{
    [Fact]
    public void LinkTwoWay_SetPropertyWithConverterOnNotConvertibleValue_LinkedPropertyIsNotUpdated()
    {
        var sourceTestData = new SourceTestData();
        var targetTestData = new TargetTestData();

        var _ = new ObjectLinkBuilder(sourceTestData, targetTestData).Build();

        sourceTestData.SecondIntValue = 1;
        targetTestData.DoubleValue = 1234;

        Assert.Equal(1, sourceTestData.SecondIntValue);
    }

    [Fact]
    public void
        LinkTwoWay_SetPropertyWithConverterToNotConvertibleValueWithConverterParameter_LinkedPropertyIsNotUpdated()
    {
        var sourceTestData = new SourceTestData();
        var targetTestData = new TargetTestData();

        var _ = new ObjectLinkBuilder(sourceTestData, targetTestData).Build();

        sourceTestData.IntValue = 1234;
        targetTestData.SecondStringValue = "Hello";

        Assert.Equal(9876, sourceTestData.IntValue);
    }

    [Fact]
    public void LinkTwoWay_SetPropertyWithConverter_LinkedPropertyIsUpdated()
    {
        var sourceTestData = new SourceTestData();
        var targetTestData = new TargetTestData();

        var _ = new ObjectLinkBuilder(sourceTestData, targetTestData).Build();

        sourceTestData.IntValue = 5678;

        Assert.Equal("5678", targetTestData.StringValue);

        targetTestData.StringValue = "1234";

        Assert.Equal(1234, sourceTestData.IntValue);
    }

    [Fact]
    public void LinkTwoWay_SetPropertyWithConverterToNotConvertibleValue_LinkedPropertyIsNotUpdated()
    {
        var sourceTestData = new SourceTestData();
        var targetTestData = new TargetTestData();

        var _ = new ObjectLinkBuilder(sourceTestData, targetTestData).Build();

        sourceTestData.IntValue = 1234;
        targetTestData.StringValue = "Hello";

        Assert.Equal(1234, sourceTestData.IntValue);
    }

    [Fact]
    public void NullableTargetConverter_SetProperty_NullablePropertyIsSet()
    {
        var sourceTestData = new NewSourceTestData();
        var targetTestData = new NewTargetTestData();

        var _ = new ObjectLinkBuilder(sourceTestData, targetTestData).Build();

        targetTestData.IsVisible = true;

        Assert.True(sourceTestData.IsVisible);

        targetTestData.IsVisible = false;

        Assert.False(sourceTestData.IsVisible);
    }

    [Fact]
    public void NullableTargetConverter_SetNullableProperty_PropertyIsSet()
    {
        var sourceTestData = new NewSourceTestData();
        var targetTestData = new NewTargetTestData();

        var _ = new ObjectLinkBuilder(sourceTestData, targetTestData).Build();

        sourceTestData.IsVisible = true;

        Assert.True(targetTestData.IsVisible);

        sourceTestData.IsVisible = false;

        Assert.False(targetTestData.IsVisible);
    }

    [Fact]
    public void NullableSourceConverter_SetProperty_NullablePropertyIsSet()
    {
        var sourceTestData = new NewSourceTestData();
        var targetTestData = new NewTargetTestData();

        var _ = new ObjectLinkBuilder(sourceTestData, targetTestData).Build();

        targetTestData.IsChecked = true;

        Assert.True(sourceTestData.IsChecked);

        targetTestData.IsChecked = false;

        Assert.False(sourceTestData.IsChecked);
    }

    [Fact]
    public void NullableSourceConverter_SetNullableProperty_PropertyIsSet()
    {
        var sourceTestData = new NewSourceTestData();
        var targetTestData = new NewTargetTestData();

        var _ = new ObjectLinkBuilder(sourceTestData, targetTestData).Build();

        sourceTestData.IsChecked = true;

        Assert.True(targetTestData.IsChecked);

        sourceTestData.IsChecked = false;

        Assert.False(targetTestData.IsChecked);
    }

    [Fact]
    public void NullableSourceConverter_SetNullablePropertyWithDefault_PropertyIsSet()
    {
        var sourceTestData = new NewSourceTestData();
        var targetTestData = new NewTargetTestData();

        var _ = new ObjectLinkBuilder(sourceTestData, targetTestData).Build();

        sourceTestData.BoolWithDefault = true;

        Assert.True(targetTestData.BoolWithDefault);

        sourceTestData.BoolWithDefault = false;

        Assert.False(targetTestData.BoolWithDefault);

        sourceTestData.BoolWithDefault = null;

        Assert.True(targetTestData.BoolWithDefault);
    }
}
