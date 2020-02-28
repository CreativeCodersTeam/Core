using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CreativeCoders.Core.Caching;
using CreativeCoders.Core.Caching.Default;

namespace CreativeCoders.Core.Enums
{
    public class EnumStringConverter : IEnumToStringConverter
    {
        private static readonly ICache<Type, IDictionary<Enum, string>> TextToEnumMappingCache =
            CacheManager.CreateCache<Type, IDictionary<Enum, string>>();

        private static readonly ICache<FieldInfo, IEnumStringAttribute> EnumToTextCache =
            new DictionaryCache<FieldInfo, IEnumStringAttribute>();

        public string Convert(Enum enumValue)
        {
            if (enumValue == null)
            {
                return string.Empty;
            }

            var fieldInfo = EnumUtils.GetFieldInfoForEnum(enumValue);

            return GetTextForField(fieldInfo);
        }

        public T Convert<T>(string text)
            where T : Enum
        {
            var mappingDict = TextToEnumMappingCache.GetOrAdd(typeof(T), () => EnumUtils.GetEnumFieldInfos<T>()
                .ToDictionary(entry => entry.Key, entry => GetTextForField(entry.Value)));

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

        private static string GetTextForField(FieldInfo fieldInfo)
        {
            var attr = EnumToTextCache.GetOrAdd(fieldInfo, () => GetEnumStringAttribute(fieldInfo));

            return attr != null ? attr.Text : fieldInfo.Name;
        }

        // ReSharper disable once SuggestBaseTypeForParameter
        private static IEnumStringAttribute GetEnumStringAttribute(FieldInfo fieldInfo)
        {
            var attrs = fieldInfo?.GetCustomAttributes(true);

            if (attrs?.FirstOrDefault(x => x is IEnumStringAttribute) is IEnumStringAttribute attr)
            {
                return attr;
            }

            return null;
        }

        public static void ClearCaches()
        {
            EnumToTextCache.Clear();
            TextToEnumMappingCache.Clear();
        }
    }
}