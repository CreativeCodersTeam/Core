using System;
using System.IO;
using System.Linq.Expressions;
using CreativeCoders.Core.Reflection;
using JetBrains.Annotations;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Reflection;

public class ReflectionTests
{
    [Fact]
    public void ReflectionTestGetMemberName()
    {
        Assert.Equal("Text", ExpressionExtensions.GetMemberName(() => Text));
        Assert.Equal("Text", ExpressionExtensions.GetMemberName<ReflectionTests, string>(x => x.Text));
    }

    [Fact]
    public void IsPropertyOf_PropertyOfType_ReturnsTrue()
    {
        Expression<Func<ReflectionTests, string>> expression = x => x.TestProp;

        Assert.True(expression.IsPropertyOf());
    }

    [Fact]
    public void IsPropertyOf_PropertyOfWrongType_ReturnsFalse()
    {
        Expression<Func<ReflectionTests, string>> expression = x => new FileInfo("Test").Name;

        Assert.False(expression.IsPropertyOf());
    }

    [Fact]
    public void IsPropertyOf_PropertyMethodOfType_ReturnsFalse()
    {
        Expression<Func<ReflectionTests, string>> expression = x => x.TestProp.ToString();

        Assert.False(expression.IsPropertyOf());
    }

    [UsedImplicitly]
    public string Text;

    [UsedImplicitly]
    public string TestProp { get; set; }
}