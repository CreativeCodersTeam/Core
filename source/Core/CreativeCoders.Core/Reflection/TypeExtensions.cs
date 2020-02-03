using System;

namespace CreativeCoders.Core.Reflection
{
    public static class TypeExtensions
    {
        public static object CreateGenericInstance(this Type type, Type typeArgument, params object[] constructorParameters)
        {
            Ensure.That(type.IsGenericType, nameof(type));
            Ensure.IsNotNull(typeArgument, nameof(typeArgument));

            var newType = type.MakeGenericType(typeArgument);
            var instance = Activator.CreateInstance(newType, constructorParameters);

            return instance;
        }

        public static object GetDefault(this Type type)
        {
            return type.IsValueType
                ? Activator.CreateInstance(type)
                : null;
        }
    }
}