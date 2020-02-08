using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CreativeCoders.Core.Reflection
{
    public static class ReflectionUtils
    {
        public static IEnumerable<Assembly> GetAllAssemblies()
        {
            var assemblies = AppDomain
                .CurrentDomain
                .GetAssemblies();

            return assemblies;
        }

        public static IEnumerable<Type> GetAllTypes()
        {
            return 
                GetAllAssemblies()
                    .SelectMany(assembly => assembly.GetTypesSafe());
        }

        public static IEnumerable<Type> GetAllTypes(Func<Assembly, bool> checkAssemblyFunc)
        {
            return
                GetAllAssemblies()
                    .Where(checkAssemblyFunc)
                    .SelectMany(assembly => assembly.GetTypesSafe());
        }
    }
}