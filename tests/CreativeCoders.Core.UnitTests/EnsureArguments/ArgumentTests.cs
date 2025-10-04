using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AwesomeAssertions;
using Xunit;

#nullable enable

namespace CreativeCoders.Core.UnitTests.EnsureArguments;

[SuppressMessage("csharp", "S3236", Justification = "Test")]
public class ArgumentTests
{
    [Fact]
    public void Cast_StringValueToEnumerableOfChar_EnumerableForStringIsReturned()
    {
        const string text = "1234";

        var argument = Ensure.Argument(text, nameof(text));

        // Act
        var value = argument.Cast<IEnumerable<char>>();

        // Arrange
        value
            .Should()
            .HaveCount(text.Length);

        value?.ToString()
            .Should()
            .Be(text);
    }

    [Fact]
    public void ImplicitCast_StringValueToEnumerableOfChar_EnumerableForStringIsReturned()
    {
        const string text = "1234";

        var argument = Ensure.Argument(text, nameof(text));

        // Act
        string? value = argument;

        // Arrange
        value
            .Should()
            .Be(text);
    }
}
