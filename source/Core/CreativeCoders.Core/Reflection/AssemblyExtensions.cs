using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace CreativeCoders.Core.Reflection;

/// <summary>
/// Provides extension methods for the <see cref="Assembly"/> class.
/// </summary>
[ExcludeFromCodeCoverage]
public static class AssemblyExtensions
{
    /// <summary>
    /// Gets all types defined in the assembly, returning an empty array instead of throwing
    /// a <see cref="ReflectionTypeLoadException"/> when types cannot be loaded.
    /// </summary>
    /// <param name="assembly">The assembly from which to retrieve types.</param>
    /// <returns>
    /// An array of all <see cref="Type"/> objects defined in the assembly, or an empty array
    /// if a <see cref="ReflectionTypeLoadException"/> occurs.
    /// </returns>
    // ReSharper disable once ReturnTypeCanBeEnumerable.Global
    public static Type[] GetTypesSafe(this Assembly assembly)
    {
        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException)
        {
            return Type.EmptyTypes;
        }
    }
}
