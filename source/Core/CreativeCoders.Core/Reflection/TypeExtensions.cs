using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

#nullable enable
namespace CreativeCoders.Core.Reflection;

/// <summary>
/// Provides extension methods for the <see cref="Type"/> class to simplify common reflection tasks.
/// </summary>
[PublicAPI]
public static class TypeExtensions
{
    /// <summary>
    /// Creates an instance of a generic type constructed with the specified type argument.
    /// </summary>
    /// <param name="type">The open generic type definition.</param>
    /// <param name="typeArgument">The type argument used to construct the generic type.</param>
    /// <param name="constructorParameters">The parameters passed to the constructor.</param>
    /// <returns>The newly created instance, or <see langword="null"/> if creation fails.</returns>
    public static object? CreateGenericInstance(this Type type, Type typeArgument,
        params object[] constructorParameters)
    {
        Ensure.That(type.IsGenericType);
        Ensure.IsNotNull(typeArgument);

        var newType = type.MakeGenericType(typeArgument);
        var instance = Activator.CreateInstance(newType, constructorParameters);

        return instance;
    }

    /// <summary>
    /// Gets the default value for the specified <see cref="Type"/>.
    /// </summary>
    /// <param name="type">The type for which to get the default value.</param>
    /// <returns>The default value for value types, or <see langword="null"/> for reference types.</returns>
    public static object? GetDefault(this Type type)
    {
        return type.IsValueType
            ? Activator.CreateInstance(type)
            : null;
    }

    /// <summary>
    /// Gets all non-abstract types in the current application domain that implement or inherit from the specified type.
    /// </summary>
    /// <param name="type">The base type or interface to find implementations for.</param>
    /// <returns>A sequence of non-abstract types assignable to the specified type.</returns>
    public static IEnumerable<Type> GetImplementations(this Type type)
    {
        return ReflectionUtils
            .GetAllTypes()
            .Where(x => !x.IsAbstract && type.IsAssignableFrom(x));
    }

    /// <summary>
    /// Gets all non-abstract types in the current application domain that implement or inherit from the specified type,
    /// optionally including reflection-only assemblies.
    /// </summary>
    /// <param name="type">The base type or interface to find implementations for.</param>
    /// <param name="withReflectionOnlyAssemblies">
    /// <see langword="true"/> to include reflection-only assemblies; otherwise, <see langword="false"/>.
    /// </param>
    /// <returns>A sequence of non-abstract types assignable to the specified type.</returns>
    public static IEnumerable<Type> GetImplementations(this Type type, bool withReflectionOnlyAssemblies)
    {
        return ReflectionUtils
            .GetAllTypes(withReflectionOnlyAssemblies)
            .Where(x => !x.IsAbstract && type.IsAssignableFrom(x));
    }

    /// <summary>
    /// Gets all non-abstract types from the specified assemblies that implement or inherit from the specified type.
    /// </summary>
    /// <param name="type">The base type or interface to find implementations for.</param>
    /// <param name="assemblies">The assemblies to search for implementations.</param>
    /// <returns>A sequence of non-abstract types assignable to the specified type.</returns>
    public static IEnumerable<Type> GetImplementations(this Type type, params Assembly[] assemblies)
    {
        return assemblies
            .SelectMany(assembly => assembly.GetTypesSafe())
            .Where(x => !x.IsAbstract && type.IsAssignableFrom(x));
    }

    /// <summary>
    /// Creates an instance of the specified type by resolving constructor parameters from a service provider.
    /// </summary>
    /// <typeparam name="T">The type of instance to create.</typeparam>
    /// <param name="type">The concrete type to instantiate.</param>
    /// <param name="serviceProvider">The service provider used to resolve constructor dependencies.</param>
    /// <param name="args">Additional arguments passed to the constructor.</param>
    /// <returns>The created instance, or <see langword="null"/> if no suitable constructor is found.</returns>
    public static T? CreateInstance<T>(this Type type, IServiceProvider serviceProvider, params object[] args)
        where T : class
    {
        return type.GetConstructors()
            .Select(constructorInfo => CreateInstance<T>(type, constructorInfo, serviceProvider, args))
            .FirstOrDefault(instance => instance != null);
    }

    /// <summary>
    /// Creates an instance of the specified type using a specific constructor, resolving parameters from a service provider.
    /// </summary>
    /// <typeparam name="T">The type of instance to create.</typeparam>
    /// <param name="type">The concrete type to instantiate.</param>
    /// <param name="ctorInfo">The constructor to invoke.</param>
    /// <param name="serviceProvider">The service provider used to resolve constructor dependencies.</param>
    /// <param name="args">Additional arguments passed to the constructor.</param>
    /// <returns>The created instance, or <see langword="null"/> if not all constructor arguments can be resolved.</returns>
    public static T? CreateInstance<T>(this Type type, ConstructorInfo ctorInfo,
        IServiceProvider serviceProvider, params object[] args)
        where T : class
    {
        var arguments = ctorInfo
            .GetParameters()
            .CreateArguments(serviceProvider, out var allArgsMatched, args);

        if (!allArgsMatched)
        {
            return null;
        }

        return Activator.CreateInstance(type, arguments) as T;
    }

    /// <summary>
    /// Determines whether the type implements a specific open generic interface.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <param name="genericInterfaceDefinition">The open generic interface definition (e.g., <c>typeof(IEnumerable&lt;&gt;)</c>).</param>
    /// <returns>
    /// <see langword="true"/> if the type implements the generic interface; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool ImplementsGenericInterface(this Type type, Type genericInterfaceDefinition) =>
        type.GetInterfaces().Any(i =>
            i.IsGenericType && i.GetGenericTypeDefinition() == genericInterfaceDefinition);

    /// <summary>
    /// Gets the generic type arguments of a specific generic interface implemented by the type.
    /// </summary>
    /// <param name="type">The type to inspect.</param>
    /// <param name="genericInterfaceDefinition">The open generic interface definition to match.</param>
    /// <returns>
    /// An array of <see cref="Type"/> objects representing the generic arguments of the matched interface,
    /// or an empty array if the interface is not implemented.
    /// </returns>
    public static Type[] GetGenericInterfaceArguments(this Type type, Type genericInterfaceDefinition)
    {
        return type.GetInterfaces()
            .FirstOrDefault(i =>
                i.IsGenericType && i.GetGenericTypeDefinition() == genericInterfaceDefinition)?
            .GetGenericArguments() ?? [];
    }
}
