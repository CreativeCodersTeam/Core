using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CreativeCoders.Core.Enums;

public static class EnumUtils
{
    // ReSharper disable once ReturnTypeCanBeEnumerable.Global
    public static FieldInfo[] GetValuesFieldInfos<T>()
        where T : Enum
    {
        var enumType = typeof(T);
        var enumValues = Enum.GetValues(enumType);

        return enumValues
            .Cast<Enum>()
            .Select(enumValue => enumType.GetField(enumValue.ToString()))
            .Where(fieldInfo => fieldInfo != null)
            .ToArray();
    }

    public static Enum[] GetEnumValues<T>()
        where T : Enum
    {
        var values = Enum.GetValues(typeof(T));
        return values.Cast<Enum>().ToArray();
    }

    public static T GetEnum<T>(int enumIntValue)
        where T : Enum
    {
        var enumValues = GetEnumValues<T>();

        foreach (var enumValue in enumValues.Where(enumValue => Convert.ToInt32(enumValue) == enumIntValue))
        {
            return (T) enumValue;
        }

        return (T) enumValues.First();
    }

    public static FieldInfo GetFieldInfoForEnum(Enum enumValue)
    {
        Ensure.IsNotNull(enumValue, nameof(enumValue));

        var enumType = enumValue.GetType();
        return enumType.GetField(enumValue.ToString());
    }

    public static IDictionary<Enum, FieldInfo> GetEnumFieldInfos(Type enumType)
    {
        if (!enumType.IsEnum)
        {
            return new Dictionary<Enum, FieldInfo>();
        }

        var enumValues = Enum.GetValues(enumType);

        var enumFieldInfos = new Dictionary<Enum, FieldInfo>();

        foreach (Enum enumValue in enumValues)
        {
            var fieldInfo = enumType.GetField(enumValue.ToString());

            if (fieldInfo != null)
            {
                enumFieldInfos.Add(enumValue, fieldInfo);
            }
        }

        return enumFieldInfos;
    }

    public static IDictionary<Enum, FieldInfo> GetEnumFieldInfos<T>()
        where T : Enum
    {
        return EnumUtils.GetEnumFieldInfos(typeof(T));
    }
}
