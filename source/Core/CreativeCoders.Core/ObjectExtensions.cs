using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
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

        [return: System.Diagnostics.CodeAnalysis.NotNull]
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

        public static async ValueTask TryDisposeAsync(this object instance)
        {
            switch (instance)
            {
                case IAsyncDisposable asyncDisposable:
                    await asyncDisposable.DisposeAsync();
                    break;
                case IDisposable disposable:
                    disposable.Dispose();
                    break;
            }
        }

        public static T GetPropertyValue<T>(this object instance, string propertyName)
        {
            var propInfo = instance.GetType().GetProperty(propertyName);

            if (propInfo == null)
            {
                throw new MissingMemberException(instance.GetType().Name, propertyName);
            }

            return (T) propInfo.GetValue(instance);
        }

        public static void SetPropertyValue<T>(this object instance, string propertyName, T value)
        {
            var propInfo = instance.GetType().GetProperty(propertyName);

            if (propInfo == null)
            {
                throw new MissingMemberException(instance.GetType().Name, propertyName);
            }

            propInfo.SetValue(instance, value);
        }
    }
}
