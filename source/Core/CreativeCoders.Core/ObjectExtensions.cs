using JetBrains.Annotations;

namespace CreativeCoders.Core
{
    [PublicAPI]
    public static class ObjectExtensions
    {
        public static string ToStringSafe(this object obj, string defaultValue)
        {
            return obj?.ToString() ?? defaultValue;
        }

        public static string ToStringSafe(this object obj)
        {
            return obj.ToStringSafe(string.Empty);
        }

        public static T As<T>(this object instance, T defaultValue)
        {
            if (instance is T value)
            {
                return value;
            }

            return defaultValue;
        }

        public static T As<T>(this object instance) => As<T>(instance, default);
    }
}