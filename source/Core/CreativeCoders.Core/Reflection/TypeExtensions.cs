using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

#nullable enable
namespace CreativeCoders.Core.Reflection;

[PublicAPI]
public static class TypeExtensions
{
    public static object? CreateGenericInstance(this Type type, Type typeArgument, params object[] constructorParameters)
    {
        Ensure.That(type.IsGenericType, nameof(type));
        Ensure.IsNotNull(typeArgument, nameof(typeArgument));

        var newType = type.MakeGenericType(typeArgument);
        var instance = Activator.CreateInstance(newType, constructorParameters);

        return instance;
    }

    public static object? GetDefault(this Type type)
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

    public static T? CreateInstance<T>(this Type type, IServiceProvider serviceProvider, params object[] args)
        where T : class
    {
        return type.GetConstructors()
            .Select(constructorInfo => CreateInstance<T>(type, constructorInfo, serviceProvider, args))
            .FirstOrDefault(instance => instance != null);
    }

    public static T? CreateInstance<T>(this Type type, ConstructorInfo ctorInfo,
        IServiceProvider serviceProvider,
        params object[] args)
        where T : class
    {
        var argList = args.ToList();

        var argumentInfos = ctorInfo.GetParameters();

        var arguments = argumentInfos
            .Select(x =>
            {
                var index = argList.FindIndex(argType =>
                    x.ParameterType == argType.GetType()
                    || (argType.GetType().GetInterfaces()
                        .Any(interfaceType => interfaceType == x.ParameterType)));

                if (index == -1)
                {
                    return serviceProvider.GetService(x.ParameterType)
                           ?? throw new InvalidOperationException(
                               $"Service '{x.ParameterType.Name}' can not be resolved");
                }

                var arg = argList[index];

                argList.RemoveAt(index);

                return arg;
            })
            .ToArray();

        if (argList.Count > 0)
        {
            return default;
        }

        return Activator.CreateInstance(type, arguments) as T;
    }
}