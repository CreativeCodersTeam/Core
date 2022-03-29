using System;
using CreativeCoders.Validation;
using CreativeCoders.Validation.Rules;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Validation;

public class ObjectValidationTests
{
    [Fact]
    public void IsNull_ObjectNull_Valid()
    {
        Assert.True(IsValid(rb => rb.IsNull(), null));
    }

    [Fact]
    public void IsNull_ObjectNotNull_Invalid()
    {
        Assert.False(IsValid(rb => rb.IsNull(), new object()));
        Assert.False(IsValid(rb => rb.IsNull(), string.Empty));
        Assert.False(IsValid(rb => rb.IsNull(), new TestDataObject()));
    }

    [Theory]
    [InlineData(5, 0, 10)]
    [InlineData(1, 0, 2)]
    [InlineData(3, 1, 5)]
    [InlineData(99, 98, 100)]
    [InlineData(17, -18, 18)]
    [InlineData(17, -1, 17)]
    public void Between_IntValueBetween_Valid(int value, int min, int max)
    {
        Assert.True(IsValid(rb => rb.Between(min, max), value));
    }

    [Theory]
    [InlineData(5, 0, 4)]
    [InlineData(1, -2, 0)]
    [InlineData(3, 1, 2)]
    [InlineData(99, 102, 105)]
    [InlineData(17, -18, -1)]
    public void Between_IntValueNotBetween_Invalid(int value, int min, int max)
    {
        Assert.False(IsValid(rb => rb.Between(min, max), value));
    }

    private static bool IsValid(Action<IPropertyRuleBuilder<TestDataObject, object>> setupRule, object objValue)
    {
        var validator = new TestDataObjectValidator(v => setupRule(v.RuleFor(x => x.ObjValue)));

        var testData = new TestDataObject { ObjValue = objValue };

        var validationResult = validator.Validate(testData);

        return validationResult.IsValid;
    }

    private static bool IsValid(Action<IPropertyRuleBuilder<TestDataObject, int>> setupRule, int intValue)
    {
        var validator = new TestDataObjectValidator(v => setupRule(v.RuleFor(x => x.IntValue)));

        var testData = new TestDataObject { IntValue = intValue };

        var validationResult = validator.Validate(testData);

        return validationResult.IsValid;
    }
}