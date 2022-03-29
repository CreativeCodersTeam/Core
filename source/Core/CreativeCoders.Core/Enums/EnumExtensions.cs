using System;

namespace CreativeCoders.Core.Enums;

public static class EnumExtensions
{
    private static readonly IEnumToStringConverter EnumToStringConverter = new EnumStringConverter();

    public static string ToText(this Enum enumValue)
    {
        return EnumToStringConverter.Convert(enumValue);
    }
}