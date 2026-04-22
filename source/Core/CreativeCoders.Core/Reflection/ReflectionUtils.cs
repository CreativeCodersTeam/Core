using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CreativeCoders.Core.Reflection;

/// <summary>
/// Provides static utility methods for retrieving assemblies and types from the current application domain.
/// </summary>
public static class ReflectionUtils
{
    /// <summary>
    /// Gets all assemblies loaded in the current application domain.
    /// </summary>
    /// <returns>A sequence of all loaded <see cref="Assembly"/> instances.</returns>
    public static IEnumerable<Assembly> GetAllAssemblies()
    {
        var assemblies = AppDomain
            .CurrentDomain
            .GetAssemblies();

        return assemblies;
    }

    /// <summary>
    /// Gets all assemblies loaded in the current application domain, optionally including reflection-only assemblies.
    /// </summary>
    /// <param name="withReflectionOnlyAssemblies">
    /// <see langword="true"/> to include reflection-only assemblies; otherwise, <see langword="false"/>.
    /// </param>
    /// <returns>A sequence of loaded <see cref="Assembly"/> instances.</returns>
    public static IEnumerable<Assembly> GetAllAssemblies(bool withReflectionOnlyAssemblies)
    {
        var assemblies = AppDomain
            .CurrentDomain
            .GetAssemblies();

        if (withReflectionOnlyAssemblies)
        {
            assemblies = assemblies
                .Concat(AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies())
                .Distinct()
                .ToArray();
        }

        return assemblies;
    }

    /// <summary>
    /// Gets all types from all assemblies loaded in the current application domain.
    /// </summary>
    /// <returns>A sequence of all <see cref="Type"/> objects from all loaded assemblies.</returns>
    public static IEnumerable<Type> GetAllTypes()
    {
        return
            GetAllAssemblies()
                .SelectMany(assembly => assembly.GetTypesSafe());
    }

    /// <summary>
    /// Gets all types from all assemblies loaded in the current application domain,
    /// optionally including reflection-only assemblies.
    /// </summary>
    /// <param name="withReflectionOnlyAssemblies">
    /// <see langword="true"/> to include reflection-only assemblies; otherwise, <see langword="false"/>.
    /// </param>
    /// <returns>A sequence of all <see cref="Type"/> objects from the matching assemblies.</returns>
    public static IEnumerable<Type> GetAllTypes(bool withReflectionOnlyAssemblies)
    {
        return
            GetAllAssemblies(withReflectionOnlyAssemblies)
                .SelectMany(assembly => assembly.GetTypesSafe());
    }

    /// <summary>
    /// Gets all types from assemblies in the current application domain that satisfy the specified predicate.
    /// </summary>
    /// <param name="checkAssembly">The predicate used to filter assemblies.</param>
    /// <returns>A sequence of all <see cref="Type"/> objects from the matching assemblies.</returns>
    public static IEnumerable<Type> GetAllTypes(Func<Assembly, bool> checkAssembly)
    {
        return
            GetAllAssemblies()
                .Where(checkAssembly)
                .SelectMany(assembly => assembly.GetTypesSafe());
    }

    /// <summary>
    /// Gets all types from assemblies in the current application domain that satisfy the specified predicate,
    /// optionally including reflection-only assemblies.
    /// </summary>
    /// <param name="checkAssembly">The predicate used to filter assemblies.</param>
    /// <param name="withReflectionOnlyAssemblies">
    /// <see langword="true"/> to include reflection-only assemblies; otherwise, <see langword="false"/>.
    /// </param>
    /// <returns>A sequence of all <see cref="Type"/> objects from the matching assemblies.</returns>
    public static IEnumerable<Type> GetAllTypes(Func<Assembly, bool> checkAssembly,
        bool withReflectionOnlyAssemblies)
    {
        return
            GetAllAssemblies(withReflectionOnlyAssemblies)
                .Where(checkAssembly)
                .SelectMany(assembly => assembly.GetTypesSafe());
    }
}
