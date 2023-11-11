using CreativeCoders.Core.ObjectLinking;
using Xunit;

namespace CreativeCoders.Core.UnitTests.ObjectLinking;

public class ObjectLinkTests
{
    [Fact]
    public void LinkOneWayToTarget_SetSourceProperty_TargetPropertyIsUpdated()
    {
        var sourceTestData = new SourceTestData();
        var targetTestData = new TargetTestData();

        _ = new ObjectLinkBuilder(sourceTestData, targetTestData).Build();

        sourceTestData.SourceText = "Test";

        Assert.Equal("Test", targetTestData.TargetText);
    }

    [Fact]
    public void LinkOneWayToTarget_SetTargetProperty_SourcePropertyIsNotUpdated()
    {
        var sourceTestData = new SourceTestData();
        var targetTestData = new TargetTestData();

        _ = new ObjectLinkBuilder(sourceTestData, targetTestData).Build();

        sourceTestData.SourceText = "Test";

        targetTestData.TargetText = "1234";

        Assert.Equal("1234", targetTestData.TargetText);
        Assert.Equal("Test", sourceTestData.SourceText);
    }

    [Fact]
    public void LinkOneWayToTarget_InitialSourcePropertySetBeforeLinking_TargetPropertyIsUpdated()
    {
        var sourceTestData = new SourceTestData {SourceText = "Test"};
        var targetTestData = new TargetTestData();

        _ = new ObjectLinkBuilder(sourceTestData, targetTestData).Build();

        Assert.Equal("Test", targetTestData.TargetText);
    }

    [Fact]
    public void LinkOneWayToTarget_InitialTargetPropertySetBeforeLinking_SourcePropertyIsNotUpdated()
    {
        var sourceTestData = new SourceTestData();
        var targetTestData = new TargetTestData {TargetText = "Test"};

        _ = new ObjectLinkBuilder(sourceTestData, targetTestData).Build();

        Assert.Null(sourceTestData.SourceText);
    }

    [Fact]
    public void LinkOneWayFromTarget_SetTargetProperty_SourcePropertyIsUpdated()
    {
        var sourceTestData = new SourceTestData();
        var targetTestData = new TargetTestData();

        _ = new ObjectLinkBuilder(sourceTestData, targetTestData).Build();

        sourceTestData.SourceName = "Test";

        Assert.Equal("Test", targetTestData.FromSourceName);
    }

    [Fact]
    public void LinkOneWayToTarget_SetTargetProperty_SourcePropertyIsUpdated()
    {
        var sourceTestData = new SourceTestData();
        var targetTestData = new TargetTestData();

        _ = new ObjectLinkBuilder(sourceTestData, targetTestData).Build();

        targetTestData.TargetName = "Test";

        Assert.Equal("Test", sourceTestData.SourceName);
    }

    [Fact]
    public void LinkOneWayFromTarget_SetSourceProperty_TargetPropertyIsNotUpdated()
    {
        var sourceTestData = new SourceTestData();
        var targetTestData = new TargetTestData();

        _ = new ObjectLinkBuilder(sourceTestData, targetTestData).Build();

        targetTestData.TargetName = "1234";

        sourceTestData.SourceName = "Test";

        Assert.Equal("1234", targetTestData.TargetName);
        Assert.Equal("Test", sourceTestData.SourceName);
    }

    [Fact]
    public void LinkOneWayFromTarget_InitialTargetPropertySetBeforeLinking_SourcePropertyIsUpdated()
    {
        var sourceTestData = new SourceTestData();
        var targetTestData = new TargetTestData {TargetName = "Hello"};

        _ = new ObjectLinkBuilder(sourceTestData, targetTestData).Build();

        Assert.Equal("Hello", sourceTestData.SourceName);
    }

    [Fact]
    public void LinkOneWayFromTarget_InitialSourcePropertySetBeforeLinking_TargetPropertyIsNotUpdated()
    {
        var sourceTestData = new SourceTestData {SourceName = "Test"};
        var targetTestData = new TargetTestData();

        _ = new ObjectLinkBuilder(sourceTestData, targetTestData).Build();

        Assert.Null(targetTestData.TargetName);
    }

    [Fact]
    public void Dispose_SetSourcePropertyAfterDispose_TargetPropertyIsNotUpdated()
    {
        var sourceTestData = new SourceTestData();
        var targetTestData = new TargetTestData();

        var link = new ObjectLinkBuilder(sourceTestData, targetTestData).Build();

        link.Dispose();

        sourceTestData.SourceText = "Test";

        Assert.Null(targetTestData.TargetText);
    }

    [Fact]
    public void LinkTwoWay_SetProperty_LinkedPropertyIsUpdated()
    {
        var sourceTestData = new SourceTestData();
        var targetTestData = new TargetTestData();

        _ = new ObjectLinkBuilder(sourceTestData, targetTestData).Build();

        sourceTestData.TwoWayProperty = "Test";

        Assert.Equal("Test", targetTestData.TwoWayProperty);
        Assert.Equal("Test", sourceTestData.TwoWayProperty);

        targetTestData.TwoWayProperty = "1234";

        Assert.Equal("1234", sourceTestData.TwoWayProperty);
        Assert.Equal("1234", targetTestData.TwoWayProperty);
    }

    [Fact]
    public void LinkTwoWay_SetPropertyOnInheritedClass_LinkedPropertyIsUpdated()
    {
        var sourceTestData = new SourceTestData();
        var targetTestData = new NewTargetTestData();

        _ = new ObjectLinkBuilder(sourceTestData, targetTestData).Build();

        sourceTestData.TwoWayProperty = "Test";

        Assert.Equal("Test", targetTestData.TwoWayProperty);
        Assert.Equal("Test", sourceTestData.TwoWayProperty);

        targetTestData.TwoWayProperty = "1234";

        Assert.Equal("1234", sourceTestData.TwoWayProperty);
        Assert.Equal("1234", targetTestData.TwoWayProperty);
    }

    [Fact]
    public void LinkTwoWay_SetPropertyOnBothInheritedClasses_LinkedPropertyIsUpdated()
    {
        var sourceTestData = new NewSourceTestData();
        var targetTestData = new NewTargetTestData();

        _ = new ObjectLinkBuilder(sourceTestData, targetTestData).Build();

        sourceTestData.TwoWayProperty = "Test";
        sourceTestData.SourceName = "HelloWorld";

        Assert.Equal("Test", targetTestData.TwoWayProperty);
        Assert.Equal("Test", sourceTestData.TwoWayProperty);
        Assert.Equal("HelloWorld", targetTestData.FromSourceNameTwoWay);
        Assert.Equal("HelloWorld", sourceTestData.SourceName);

        targetTestData.TwoWayProperty = "1234";
        targetTestData.FromSourceNameTwoWay = "abcd";

        Assert.Equal("1234", sourceTestData.TwoWayProperty);
        Assert.Equal("1234", targetTestData.TwoWayProperty);
        Assert.Equal("abcd", targetTestData.FromSourceNameTwoWay);
        Assert.Equal("abcd", sourceTestData.SourceName);
    }

    [Fact]
    public void LinkOneWayToTarget_InitialPropertiesSetBeforeLinking_TargetPropertyIsUpdated()
    {
        var sourceTestData = new NewSourceTestData {InitialText = "Test"};
        var targetTestData = new NewTargetTestData {InitialText = "1234"};

        _ = new ObjectLinkBuilder(sourceTestData, targetTestData).Build();

        Assert.Equal("1234", sourceTestData.InitialText);

        sourceTestData.InitialText = "abc";
        Assert.Equal("abc", targetTestData.InitialText);
    }
}
