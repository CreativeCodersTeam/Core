using System;
using System.Collections.Generic;
using System.Linq;

namespace CreativeCoders.Core.Enums;

/// <summary>
/// Provides extension methods for <see cref="Enum"/> types.
/// </summary>
public static class EnumExtensions
{
    private static readonly EnumStringConverter __enumToStringConverter = new EnumStringConverter();

    /// <summary>
    /// Converts the enum value to its string representation using <see cref="IEnumStringAttribute"/> metadata.
    /// </summary>
    /// <param name="enumValue">The enum value to convert.</param>
    /// <returns>The string representation of the enum value.</returns>
    public static string ToText(this Enum enumValue)
    {
        return __enumToStringConverter.Convert(enumValue);
    }

    /// <summary>
    /// Enumerates all individual flag values that are set on the specified flags enum value.
    /// </summary>
    /// <typeparam name="T">The type of the flags enum.</typeparam>
    /// <param name="flags">The flags enum value to decompose.</param>
    /// <returns>A sequence of individual flag values that are set in <paramref name="flags"/>.</returns>
    public static IEnumerable<T> EnumerateFlags<T>(this T flags)
        where T : struct, Enum
    {
        return Enum
            .GetValues<T>()
            .Where(x => flags.HasFlag(x));
    }
}
