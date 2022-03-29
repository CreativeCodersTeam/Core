using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CreativeCoders.Core.Reflection;

public static class ReflectionUtils
{
    public static IEnumerable<Assembly> GetAllAssemblies()
    {
        var assemblies = AppDomain
            .CurrentDomain
            .GetAssemblies();

        return assemblies;
    }

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

    public static IEnumerable<Type> GetAllTypes()
    {
        return 
            GetAllAssemblies()
                .SelectMany(assembly => assembly.GetTypesSafe());
    }

    public static IEnumerable<Type> GetAllTypes(bool withReflectionOnlyAssemblies)
    {
        return
            GetAllAssemblies(withReflectionOnlyAssemblies)
                .SelectMany(assembly => assembly.GetTypesSafe());
    }

    public static IEnumerable<Type> GetAllTypes(Func<Assembly, bool> checkAssembly)
    {
        return
            GetAllAssemblies()
                .Where(checkAssembly)
                .SelectMany(assembly => assembly.GetTypesSafe());
    }

    public static IEnumerable<Type> GetAllTypes(Func<Assembly, bool> checkAssembly, bool withReflectionOnlyAssemblies)
    {
        return
            GetAllAssemblies(withReflectionOnlyAssemblies)
                .Where(checkAssembly)
                .SelectMany(assembly => assembly.GetTypesSafe());
    }
}