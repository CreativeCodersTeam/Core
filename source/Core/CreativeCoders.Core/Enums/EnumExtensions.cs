using System;
using System.Collections.Generic;
using System.Linq;

namespace CreativeCoders.Core.Enums;

public static class EnumExtensions
{
    private static readonly EnumStringConverter __enumToStringConverter = new EnumStringConverter();

    public static string ToText(this Enum enumValue)
    {
        return __enumToStringConverter.Convert(enumValue);
    }

    public static IEnumerable<T> EnumerateFlags<T>(this T flags)
        where T : struct, Enum
    {
        return Enum
            .GetValues<T>()
            .Where(x => flags.HasFlag(x));
    }
}
