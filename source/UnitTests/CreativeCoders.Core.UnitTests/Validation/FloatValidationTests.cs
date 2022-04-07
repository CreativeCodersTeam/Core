using System;
using CreativeCoders.Validation;
using CreativeCoders.Validation.Rules;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Validation;

public class FloatValidationTests
{
    [Theory]
    [InlineData(12, 12)]
    [InlineData(12, 11.9999999999)]
    public void IsEqual_EqualValues_Valid(double floatValue, double compareValue)
    {
        Assert.True(IsValid(rb => rb.IsEqual(compareValue), floatValue));
    }

    [Theory]
    [InlineData(12, 12.1)]
    [InlineData(12.1, 11.9999999999)]
    public void IsEqual_NotEqualValues_Invalid(double floatValue, double compareValue)
    {
        Assert.False(IsValid(rb => rb.IsEqual(compareValue), floatValue));
    }

    [Theory]
    [InlineData(12, 12)]
    [InlineData(12, 11.9999999999)]
    public void IsNotEqual_EqualValues_Invalid(double floatValue, double compareValue)
    {
        Assert.False(IsValid(rb => rb.IsNotEqual(compareValue), floatValue));
    }

    [Theory]
    [InlineData(12, 12.1)]
    [InlineData(12.1, 11.9999999999)]
    public void IsNotEqual_NotEqualValues_Valid(double floatValue, double compareValue)
    {
        Assert.True(IsValid(rb => rb.IsNotEqual(compareValue), floatValue));
    }

    [Theory]
    [InlineData(12, 12, 0.1)]
    [InlineData(12, 11.9, 0.1)]
    [InlineData(12, 11.9999999, 0.000001)]
    public void IsEqual_EqualValuesWithTolerance_Valid(double floatValue, double compareValue,
        double tolerance)
    {
        Assert.True(IsValid(rb => rb.IsEqual(compareValue, tolerance), floatValue));
    }

    [Theory]
    [InlineData(12, 12.1, 0.01)]
    [InlineData(12.1, 11.9999999999, 0.01)]
    public void IsEqual_NotEqualValuesWithTolerance_Invalid(double floatValue, double compareValue,
        double tolerance)
    {
        Assert.False(IsValid(rb => rb.IsEqual(compareValue, tolerance), floatValue));
    }

    [Theory]
    [InlineData(12, 12, 0.1)]
    [InlineData(12, 11.99, 0.1)]
    public void IsNotEqual_EqualValuesWithTolerance_Invalid(double floatValue, double compareValue,
        double tolerance)
    {
        Assert.False(IsValid(rb => rb.IsNotEqual(compareValue, tolerance), floatValue));
    }

    [Theory]
    [InlineData(12, 12.1, 0.01)]
    [InlineData(12.1, 11.9999999999, 0.00001)]
    public void IsNotEqual_NotEqualValuesWithTolerance_Valid(double floatValue, double compareValue,
        double tolerance)
    {
        Assert.True(IsValid(rb => rb.IsNotEqual(compareValue, tolerance), floatValue));
    }

    private static bool IsValid(Action<IPropertyRuleBuilder<TestDataObject, double>> setupRule,
        double floatValue)
    {
        var validator = new TestDataObjectValidator(v => setupRule(v.RuleFor(x => x.FloatValue)));

        var testData = new TestDataObject {FloatValue = floatValue};

        var validationResult = validator.Validate(testData);

        return validationResult.IsValid;
    }

    [Theory]
    [InlineData(12, 12)]
    [InlineData(12, 11.9999999999)]
    public void IsEqual_EqualIntValues_Valid(int intValue, double compareValue)
    {
        Assert.True(IsValid(rb => rb.IsEqual(compareValue), intValue));
    }

    [Theory]
    [InlineData(12, 12.1)]
    [InlineData(12, 11.99)]
    public void IsEqual_NotEqualIntValues_Invalid(int intValue, double compareValue)
    {
        Assert.False(IsValid(rb => rb.IsEqual(compareValue), intValue));
    }

    [Theory]
    [InlineData(12, 12)]
    [InlineData(12, 11.99999999)]
    public void IsNotEqual_EqualIntValues_Invalid(int intValue, double compareValue)
    {
        Assert.False(IsValid(rb => rb.IsNotEqual(compareValue), intValue));
    }

    [Theory]
    [InlineData(12, 12.1)]
    [InlineData(12, 11.99)]
    public void IsNotEqual_NotEqualIntValues_Valid(int intValue, double compareValue)
    {
        Assert.True(IsValid(rb => rb.IsNotEqual(compareValue), intValue));
    }

    private static bool IsValid(Action<IPropertyRuleBuilder<TestDataObject, int>> setupRule, int intValue)
    {
        var validator = new TestDataObjectValidator(v => setupRule(v.RuleFor(x => x.IntValue)));

        var testData = new TestDataObject {IntValue = intValue};

        var validationResult = validator.Validate(testData);

        return validationResult.IsValid;
    }

    [Theory]
    [InlineData(12, 12)]
    [InlineData(12, 11.9999999999)]
    public void IsEqual_EqualLongValues_Valid(long longValue, double compareValue)
    {
        Assert.True(IsValid(rb => rb.IsEqual(compareValue), longValue));
    }

    [Theory]
    [InlineData(12, 12.1)]
    [InlineData(12, 11.99)]
    public void IsEqual_NotEqualLongValues_Invalid(long longValue, double compareValue)
    {
        Assert.False(IsValid(rb => rb.IsEqual(compareValue), longValue));
    }

    [Theory]
    [InlineData(12, 12)]
    [InlineData(12, 11.99999999)]
    public void IsNotEqual_EqualLongValues_Invalid(long longValue, double compareValue)
    {
        Assert.False(IsValid(rb => rb.IsNotEqual(compareValue), longValue));
    }

    [Theory]
    [InlineData(12, 12.1)]
    [InlineData(12, 11.99)]
    public void IsNotEqual_NotEqualLongValues_Valid(long longValue, double compareValue)
    {
        Assert.True(IsValid(rb => rb.IsNotEqual(compareValue), longValue));
    }

    private static bool IsValid(Action<IPropertyRuleBuilder<TestDataObject, long>> setupRule, long longValue)
    {
        var validator = new TestDataObjectValidator(v => setupRule(v.RuleFor(x => x.LongValue)));

        var testData = new TestDataObject {LongValue = longValue};

        var validationResult = validator.Validate(testData);

        return validationResult.IsValid;
    }

    [Theory]
    [InlineData(12, 12)]
    [InlineData(12, 11.9999999999)]
    public void IsEqual_EqualDecimalValues_Valid(decimal decimalValue, double compareValue)
    {
        Assert.True(IsValid(rb => rb.IsEqual(compareValue), decimalValue));
    }

    [Theory]
    [InlineData(12, 12.1)]
    [InlineData(12, 11.99)]
    public void IsEqual_NotEqualDecimalValues_Invalid(decimal decimalValue, double compareValue)
    {
        Assert.False(IsValid(rb => rb.IsEqual(compareValue), decimalValue));
    }

    [Theory]
    [InlineData(12, 12)]
    [InlineData(12, 11.99999999)]
    public void IsNotEqual_EqualDecimalValues_Invalid(decimal decimalValue, double compareValue)
    {
        Assert.False(IsValid(rb => rb.IsNotEqual(compareValue), decimalValue));
    }

    [Theory]
    [InlineData(12, 12.1)]
    [InlineData(12, 11.99)]
    public void IsNotEqual_NotEqualDecimalValues_Valid(decimal decimalValue, double compareValue)
    {
        Assert.True(IsValid(rb => rb.IsNotEqual(compareValue), decimalValue));
    }

    private static bool IsValid(Action<IPropertyRuleBuilder<TestDataObject, decimal>> setupRule,
        decimal decimalValue)
    {
        var validator = new TestDataObjectValidator(v => setupRule(v.RuleFor(x => x.DecimalValue)));

        var testData = new TestDataObject {DecimalValue = decimalValue};

        var validationResult = validator.Validate(testData);

        return validationResult.IsValid;
    }

    [Theory]
    [InlineData("12", 12)]
    [InlineData("12", 11.9999999999)]
    public void IsEqual_EqualStrValues_Valid(string strValue, double compareValue)
    {
        Assert.True(IsValid(rb => rb.IsEqual(compareValue), strValue));
    }

    [Theory]
    [InlineData("12", 12.1)]
    [InlineData("12", 11.99)]
    [InlineData("xx", 12)]
    public void IsEqual_NotEqualStrValues_Invalid(string strValue, double compareValue)
    {
        Assert.False(IsValid(rb => rb.IsEqual(compareValue), strValue));
    }

    [Theory]
    [InlineData("12", 12)]
    [InlineData("12", 11.99999999)]
    public void IsNotEqual_EqualStrValues_Invalid(string strValue, double compareValue)
    {
        Assert.False(IsValid(rb => rb.IsNotEqual(compareValue), strValue));
    }

    [Theory]
    [InlineData("12", 12.1)]
    [InlineData("12", 11.99)]
    public void IsNotEqual_NotEqualStrValues_Valid(string strValue, double compareValue)
    {
        Assert.True(IsValid(rb => rb.IsNotEqual(compareValue), strValue));
    }

    private static bool IsValid(Action<IPropertyRuleBuilder<TestDataObject, string>> setupRule,
        string strValue)
    {
        var validator = new TestDataObjectValidator(v => setupRule(v.RuleFor(x => x.StrValue)));

        var testData = new TestDataObject {StrValue = strValue};

        var validationResult = validator.Validate(testData);

        return validationResult.IsValid;
    }

    [Fact]
    public void IsEqual_NotEqualObjValues_Invalid()
    {
        Assert.False(IsValid(rb => rb.IsEqual(0.1), new object()));
    }

    private static bool IsValid(Action<IPropertyRuleBuilder<TestDataObject, object>> setupRule,
        object objValue)
    {
        var validator = new TestDataObjectValidator(v => setupRule(v.RuleFor(x => x.ObjValue)));

        var testData = new TestDataObject {ObjValue = objValue};

        var validationResult = validator.Validate(testData);

        return validationResult.IsValid;
    }
}
