using System;
using System.Diagnostics.CodeAnalysis;
using CreativeCoders.Validation;
using CreativeCoders.Validation.Rules;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Validation;

[SuppressMessage("ReSharper", "StringLiteralTypo")]
public class TextValidationTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void IsNotNullOrWhitespace_StrValueIsNullOrWhitespace_Invalid(string strValue)
    {
        Assert.False(IsValid(v => v.IsNotNullOrWhiteSpace(), strValue));
    }

    [Theory]
    [InlineData("Test")]
    [InlineData("123456")]
    [InlineData("Text")]
    public void IsNotNullOrWhitespace_StrValueIsNullOrWhitespace_Valid(string strValue)
    {
        Assert.True(IsValid(v => v.IsNotNullOrWhiteSpace(), strValue));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void IsNullOrWhitespace_StrValueIsNullOrWhitespace_Valid(string strValue)
    {
        Assert.True(IsValid(v => v.IsNullOrWhiteSpace(), strValue));
    }

    [Theory]
    [InlineData("Test")]
    [InlineData("123456")]
    [InlineData("Text")]
    public void IsNullOrWhitespace_StrValueIsNullOrWhitespace_Invalid(string strValue)
    {
        Assert.False(IsValid(v => v.IsNullOrWhiteSpace(), strValue));
    }

    [Theory]
    [InlineData("Hello")]
    [InlineData("12345")]
    [InlineData("     ")]
    public void HasLength_StrValueHasCorrectLength_Valid(string strValue)
    {
        Assert.True(IsValid(v => v.HasLength(5), strValue));
    }

    [Theory]
    [InlineData("Hell")]
    [InlineData("123456")]
    [InlineData((string)null)]
    public void HasLength_StrValueHasWrongLength_Invalid(string strValue)
    {
        Assert.False(IsValid(v => v.HasLength(5), strValue));
    }

    [Theory]
    [InlineData("Hello")]
    [InlineData("123456")]
    [InlineData("       ")]
    [InlineData("Test1234")]
    public void HasLengthBetween_StrValueHasCorrectLength_Valid(string strValue)
    {
        Assert.True(IsValid(v => v.HasLength(5, 8), strValue));
    }

    [Theory]
    [InlineData("Hell")]
    [InlineData("123456789")]
    [InlineData((string)null)]
    public void HasLengthBetween_StrValueHasWrongLength_Invalid(string strValue)
    {
        Assert.False(IsValid(v => v.HasLength(5, 8), strValue));
    }

    [Theory]
    [InlineData("Hello", 5)]
    [InlineData("123456", 6)]
    [InlineData("       ", 7)]
    [InlineData("Test1234", 8)]
    public void HasLengthFunc_StrValueHasCorrectLength_Valid(string strValue, int length)
    {
        Assert.True(IsValid(v => v.HasLength((_, _) => length), strValue));
    }

    [Theory]
    [InlineData("Hell", 5)]
    [InlineData("123456789", 6)]
    [InlineData(null, 2)]
    public void HasLengthFunc_StrValueHasWrongLength_Invalid(string strValue, int length)
    {
        Assert.False(IsValid(v => v.HasLength((_, _) => length), strValue));
    }

    [Theory]
    [InlineData("Hello")]
    [InlineData("123456")]
    [InlineData("      ")]
    public void HasMinimumLength_StrValueHasMinLength_Valid(string strValue)
    {
        Assert.True(IsValid(v => v.HasMinimumLength(5), strValue));
    }

    [Theory]
    [InlineData("Hell")]
    [InlineData("123")]
    [InlineData((string)null)]
    public void HasMinimumLength_StrValueNotHasMinLength_Invalid(string strValue)
    {
        Assert.False(IsValid(v => v.HasMinimumLength(5), strValue));
    }

    [Theory]
    [InlineData("Hello")]
    [InlineData("1234")]
    [InlineData((string)null)]
    public void HasMaximumLength_StrValueHasMaxLength_Valid(string strValue)
    {
        Assert.True(IsValid(v => v.HasMaximumLength(5), strValue));
    }

    [Theory]
    [InlineData("Helloo")]
    [InlineData("1234567")]
    [InlineData("         ")]
    public void HasMaximumLength_StrValueNotHasMaxLength_Invalid(string strValue)
    {
        Assert.False(IsValid(v => v.HasMaximumLength(5), strValue));
    }

    [Theory]
    [InlineData("12Helloo")]
    [InlineData("1234567")]
    [InlineData("12         ")]
    public void StartsWith_StrValue_Valid(string strValue)
    {
        Assert.True(IsValid(v => v.StartsWith("12"), strValue));
    }

    [Theory]
    [InlineData("xx12Helloo")]
    [InlineData("x1234567")]
    [InlineData(" 12         ")]
    [InlineData(null)]
    public void StartsWith_StrValue_Invalid(string strValue)
    {
        Assert.False(IsValid(v => v.StartsWith("12"), strValue));
    }

    [Theory]
    [InlineData("Helloo12")]
    [InlineData("3456712")]
    [InlineData("         12")]
    public void EndsWith_StrValue_Valid(string strValue)
    {
        Assert.True(IsValid(v => v.EndsWith("12"), strValue));
    }

    [Theory]
    [InlineData("Helloo12x")]
    [InlineData("3456712xx")]
    [InlineData("        12 ")]
    [InlineData(null)]
    public void EndsWith_StrValue_Invalid(string strValue)
    {
        Assert.False(IsValid(v => v.EndsWith("12"), strValue));
    }

    [Theory]
    [InlineData("Helloo12x", "lloo")]
    [InlineData("3456712xx", "345")]
    [InlineData("        12 ", "12 ")]
    public void Contains_StrValue_Valid(string strValue, string findText)
    {
        Assert.True(IsValid(v => v.Contains(findText), strValue));
    }

    [Theory]
    [InlineData("Helloo12x", "llloo")]
    [InlineData("3456712xx", "3457")]
    [InlineData("        12 ", "1 2 ")]
    public void Contains_StrValue_Invalid(string strValue, string findText)
    {
        Assert.False(IsValid(v => v.Contains(findText), strValue));
    }

    [Theory]
    [InlineData("Helloo12x", "Helloo12")]
    [InlineData("3456712xx", "3456712x")]
    [InlineData("        12 ", "        12")]
    [InlineData(null, "Helloo12")]
    [InlineData("Helloo12x", null)]
    public void IsEqual_StrValue_Invalid(string strValue, string compareText)
    {
        Assert.False(IsValid(v => v.IsEqual(compareText), strValue));
    }

    [Theory]
    [InlineData("Helloo12x", "Helloo12x")]
    [InlineData("3456712xx", "3456712xx")]
    [InlineData("        12 ", "        12 ")]
    [InlineData(null, null)]
    public void IsEqual_StrValue_Valid(string strValue, string compareText)
    {
        Assert.True(IsValid(v => v.IsEqual(compareText), strValue));
    }

    [Theory]
    [InlineData("Helloo12x", "Helloo12")]
    [InlineData("3456712xx", "3456712x")]
    [InlineData("        12 ", "        12")]
    [InlineData(null, "Helloo12")]
    [InlineData("Helloo12x", null)]
    public void IsNotEqual_StrValue_Valid(string strValue, string compareText)
    {
        Assert.True(IsValid(v => v.IsNotEqual(compareText), strValue));
    }

    [Theory]
    [InlineData("Helloo12x", "Helloo12x")]
    [InlineData("3456712xx", "3456712xx")]
    [InlineData("        12 ", "        12 ")]
    [InlineData(null, null)]
    public void IsNotEqual_StrValue_Invalid(string strValue, string compareText)
    {
        Assert.False(IsValid(v => v.IsNotEqual(compareText), strValue));
    }

    [Theory]
    [InlineData("Helloo12x", "Helloo12", StringComparison.CurrentCulture)]
    [InlineData("Helloo12x", "Helloo12", StringComparison.CurrentCultureIgnoreCase)]
    [InlineData("3456712xx", "3456712x", StringComparison.CurrentCulture)]
    [InlineData("3456712xx", "3456712x", StringComparison.CurrentCultureIgnoreCase)]
    [InlineData("        12 ", "        12", StringComparison.CurrentCulture)]
    [InlineData("        12 ", "        12", StringComparison.CurrentCultureIgnoreCase)]
    [InlineData(null, "Helloo12", StringComparison.CurrentCulture)]
    [InlineData(null, "Helloo12", StringComparison.CurrentCultureIgnoreCase)]
    [InlineData("Helloo12x", null, StringComparison.CurrentCulture)]
    [InlineData("Helloo12x", null, StringComparison.CurrentCultureIgnoreCase)]
    public void IsEqual_StrValueWithComparison_Invalid(string strValue, string compareText, StringComparison comparison)
    {
        Assert.False(IsValid(v => v.IsEqual(compareText, comparison), strValue));
    }

    [Theory]
    [InlineData("Helloo12x", "Helloo12x", StringComparison.CurrentCulture)]
    [InlineData("3456712xx", "3456712xx", StringComparison.CurrentCulture)]
    [InlineData("        12 ", "        12 ", StringComparison.CurrentCulture)]
    [InlineData(null, null, StringComparison.CurrentCulture)]
    [InlineData("Helloo12x", "helloo12x", StringComparison.CurrentCultureIgnoreCase)]
    [InlineData("3456712XX", "3456712xx", StringComparison.CurrentCultureIgnoreCase)]
    [InlineData("AbCd 12 ", "abcd 12 ", StringComparison.CurrentCultureIgnoreCase)]
    [InlineData(null, null, StringComparison.CurrentCultureIgnoreCase)]
    public void IsEqual_StrValueWithComparison_Valid(string strValue, string compareText, StringComparison comparison)
    {
        Assert.True(IsValid(v => v.IsEqual(compareText, comparison), strValue));
    }

    [Theory]
    [InlineData("Helloo12x", "Helloo12", StringComparison.CurrentCulture)]
    [InlineData("Helloo12x", "Helloo12", StringComparison.CurrentCultureIgnoreCase)]
    [InlineData("3456712xx", "3456712x", StringComparison.CurrentCulture)]
    [InlineData("3456712xx", "3456712x", StringComparison.CurrentCultureIgnoreCase)]
    [InlineData("        12 ", "        12", StringComparison.CurrentCulture)]
    [InlineData("        12 ", "        12", StringComparison.CurrentCultureIgnoreCase)]
    [InlineData(null, "Helloo12", StringComparison.CurrentCulture)]
    [InlineData(null, "Helloo12", StringComparison.CurrentCultureIgnoreCase)]
    [InlineData("Helloo12x", null, StringComparison.CurrentCulture)]
    [InlineData("Helloo12x", null, StringComparison.CurrentCultureIgnoreCase)]
    public void IsNotEqual_StrValueComparison_Valid(string strValue, string compareText, StringComparison comparison)
    {
        Assert.True(IsValid(v => v.IsNotEqual(compareText, comparison), strValue));
    }

    [Theory]
    [InlineData("Helloo12x", "Helloo12x", StringComparison.CurrentCulture)]
    [InlineData("3456712xx", "3456712xx", StringComparison.CurrentCulture)]
    [InlineData("        12 ", "        12 ", StringComparison.CurrentCulture)]
    [InlineData(null, null, StringComparison.CurrentCulture)]
    [InlineData("Helloo12x", "helloo12x", StringComparison.CurrentCultureIgnoreCase)]
    [InlineData("3456712XX", "3456712xx", StringComparison.CurrentCultureIgnoreCase)]
    [InlineData("AbCd 12 ", "abcd 12 ", StringComparison.CurrentCultureIgnoreCase)]
    [InlineData(null, null, StringComparison.CurrentCultureIgnoreCase)]
    public void IsNotEqual_StrValueComparison_Invalid(string strValue, string compareText, StringComparison comparison)
    {
        Assert.False(IsValid(v => v.IsNotEqual(compareText, comparison), strValue));
    }

    // ReSharper disable once MemberCanBeMadeStatic.Local
    private bool IsValid(Action<IPropertyRuleBuilder<TestDataObject, string>> setupRule, string strValue)
    {
        var validator = new TestDataObjectValidator(v => setupRule(v.RuleFor(x => x.StrValue)));

        var testData = new TestDataObject {StrValue = strValue};

        var validationResult = validator.Validate(testData);

        return validationResult.IsValid;
    }
}