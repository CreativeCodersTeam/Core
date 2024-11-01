using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CreativeCoders.Core.Caching;
using CreativeCoders.Core.Caching.Default;
using CreativeCoders.Core.Collections;

namespace CreativeCoders.Core.Enums;

public class EnumStringConverter : IEnumToStringConverter
{
    private static readonly ICache<Type, IDictionary<Enum, string>> __textToEnumMappingCache =
        CacheManager.CreateCache<Type, IDictionary<Enum, string>>();

    private static readonly DictionaryCache<FieldInfo, IEnumStringAttribute> __enumToTextCache =
        new DictionaryCache<FieldInfo, IEnumStringAttribute>();

    private static string GetTextForField(FieldInfo fieldInfo, Enum enumValue)
    {
        var attr =
            __enumToTextCache.GetOrAdd(fieldInfo, () => GetEnumStringAttribute(fieldInfo))
            ?? GetEnumStringAttribute(enumValue.GetType());

        return attr != null ? attr.Text : fieldInfo.Name;
    }


    private static IEnumStringAttribute GetEnumStringAttribute(ICustomAttributeProvider attributeProvider)
    {
        var attrs = attributeProvider?.GetCustomAttributes(true);

        if (attrs?.FirstOrDefault(x => x is IEnumStringAttribute) is IEnumStringAttribute attr)
        {
            return attr;
        }

        return null;
    }

    public static void ClearCaches()
    {
        __enumToTextCache.Clear();
        __textToEnumMappingCache.Clear();
    }

    public string Convert(Enum enumValue)
    {
        if (enumValue == null)
        {
            return string.Empty;
        }

        var fieldInfo = EnumUtils.GetFieldInfoForEnum(enumValue);

        return GetTextForField(fieldInfo, enumValue);
    }

    public T Convert<T>(string text)
        where T : Enum
    {
        var mappingDict = __textToEnumMappingCache.GetOrAdd(typeof(T), () => EnumUtils.GetEnumFieldInfos<T>()
            .ToDictionary(entry => entry.Key, entry => GetTextForField(entry.Value, entry.Key)));

        if (!mappingDict.TryGetKeyByValue(text, out var returnValue))
        {
            return default;
        }

        if (returnValue is T enumValue)
        {
            return enumValue;
        }

        return default;
    }
}
