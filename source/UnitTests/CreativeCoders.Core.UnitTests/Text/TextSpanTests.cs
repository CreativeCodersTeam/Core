using System;
using CreativeCoders.Core.Text;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Text;

public class TextSpanTests
{
    [Fact]
    public void TextSpanCtorTest()
    {
        Assert.Throws<ArgumentException>(() => new TextSpan(-1, 10));
        Assert.Throws<ArgumentException>(() => new TextSpan(0, -1));
        var _ = new TextSpan(0, 10);
    }

    [Fact]
    public void IsEmptyTestTrue()
    {
        var textSpan = new TextSpan(10, 0);
        Assert.True(textSpan.IsEmpty);            
    }

    [Fact]
    public void IsEmptyTestFalse()
    {
        var textSpan = new TextSpan(10, 1);
        Assert.False(textSpan.IsEmpty);
    }

    [Fact]
    public void EndTest()
    {
        var textSpan = new TextSpan(10, 1);
        Assert.Equal(11, textSpan.End);
    }

    [Fact]
    public void StartTest()
    {
        var textSpan = new TextSpan(10, 1);
        Assert.Equal(10, textSpan.Start);
    }
}