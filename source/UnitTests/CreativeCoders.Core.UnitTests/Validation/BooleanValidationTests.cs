using System;
using CreativeCoders.Validation;
using CreativeCoders.Validation.Rules;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Validation;

public class BooleanValidationTests
{
    [Theory]
    [InlineData(2)]
    [InlineData(4)]
    [InlineData(6)]
    [InlineData(8)]
    public void IsTrue_IntValueIsOdd_Valid(int intValue)
    {
        Assert.True(IsValid(rb => rb.IsTrue(i => i % 2 == 0), intValue));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(5)]
    [InlineData(7)]
    public void IsTrue_IntValueIsNotOdd_Invalid(int intValue)
    {
        Assert.False(IsValid(rb => rb.IsTrue(i => i % 2 == 0), intValue));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(5)]
    [InlineData(7)]
    public void IsFalse_IntValueIsOdd_Valid(int intValue)
    {
        Assert.True(IsValid(rb => rb.IsFalse(i => i % 2 == 0), intValue));
    }

    [Theory]
    [InlineData(2)]
    [InlineData(4)]
    [InlineData(6)]
    [InlineData(8)]
    public void IsFalse_IntValueIsNotOdd_Invalid(int intValue)
    {
        Assert.False(IsValid(rb => rb.IsFalse(i => i % 2 == 0), intValue));
    }

    [Theory]
    [InlineData(2)]
    [InlineData(4)]
    [InlineData(6)]
    [InlineData(8)]
    public void Must_IntValueIsOdd_Valid(int intValue)
    {
        Assert.True(IsValid(rb => rb.Must((o, i) => i % 2 == 0 && i == o.IntValue), intValue));
    }

    [Theory]
    [InlineData(2)]
    [InlineData(4)]
    [InlineData(6)]
    [InlineData(8)]
    public void MustNot_IntValueIsOdd_Invalid(int intValue)
    {
        Assert.False(IsValid(rb => rb.MustNot((o, i) => i == o.IntValue), intValue));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(5)]
    [InlineData(7)]
    public void MustNot_IntValueIsOdd_Valid(int intValue)
    {
        Assert.True(IsValid(rb => rb.MustNot((_, i) => i % 2 == 0), intValue));
    }

    private static bool IsValid(Action<IPropertyRuleBuilder<TestDataObject, int>> setupRule, int intValue)
    {
        var validator = new TestDataObjectValidator(v => setupRule(v.RuleFor(x => x.IntValue)));

        var testData = new TestDataObject {IntValue = intValue};

        var validationResult = validator.Validate(testData);

        return validationResult.IsValid;
    }
}
