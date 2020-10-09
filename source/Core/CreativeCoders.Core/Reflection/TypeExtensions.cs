using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Reflection
{
    [PublicAPI]
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

        public static IEnumerable<Type> GetImplementations(this Type type)
        {
            return ReflectionUtils
                .GetAllTypes()
                .Where(x => !x.IsAbstract && type.IsAssignableFrom(x));
        }

        public static IEnumerable<Type> GetImplementations(this Type type, bool withReflectionOnlyAssemblies)
        {
            return ReflectionUtils
                .GetAllTypes(withReflectionOnlyAssemblies)
                .Where(x => !x.IsAbstract && type.IsAssignableFrom(x));
        }

        public static IEnumerable<Type> GetImplementations(this Type type, params Assembly[] assemblies)
        {
            return assemblies
                .SelectMany(assembly => assembly.GetTypesSafe())
                .Where(x => !x.IsAbstract && type.IsAssignableFrom(x));
        }
    }
}