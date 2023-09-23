using System;
using System.Collections.Generic;
using System.Linq;

namespace CreativeCoders.Core.Enums;

public static class EnumExtensions
{
    private static readonly IEnumToStringConverter EnumToStringConverter = new EnumStringConverter();

    public static string ToText(this Enum enumValue)
    {
        return EnumToStringConverter.Convert(enumValue);
    }

    public static IEnumerable<T> EnumerateFlags<T>(this T flags)
        where T : struct, Enum
    {
        return Enum
            .GetValues<T>()
            .Where(x => flags.HasFlag(x));
    }
}
