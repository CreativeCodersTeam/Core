using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace CreativeCoders.Core.Reflection
{
    [ExcludeFromCodeCoverage]
    public static class AssemblyExtensions
    {
        // ReSharper disable once ReturnTypeCanBeEnumerable.Global
        public static Type[] GetTypesSafe(this Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException)
            {
                return new Type[0];
            }
        }
    }
}