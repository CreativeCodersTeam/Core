using System;
using System.Collections.Generic;
using System.Reflection;
using CreativeCoders.Core;
using JetBrains.Annotations;

namespace CreativeCoders.Di.Building
{
    [PublicAPI]
    public static class DiContainerBuilderExtensions
    {
        public static IDiContainerBuilder RegisterImplementations(this IDiContainerBuilder builder, IEnumerable<Type> types)
        {
            new AutoRegisterImplementations(builder).ForTypes(types).Register();

            return builder;
        }

        public static IDiContainerBuilder RegisterImplementations(this IDiContainerBuilder builder,
            Assembly assembly)
        {
            new AutoRegisterImplementations(builder).ForTypesInAssembly(assembly).Register();

            return builder;
        }

        public static IDiContainerBuilder RegisterImplementations(this IDiContainerBuilder builder,
            IEnumerable<Assembly> assemblies)
        {
            var autoRegisterImplementations = new AutoRegisterImplementations(builder);
                
            assemblies.ForEach(assembly => autoRegisterImplementations.ForTypesInAssembly(assembly));

            autoRegisterImplementations.Register();

            return builder;
        }

        public static IDiContainerBuilder RegisterImplementations(this IDiContainerBuilder builder)
        {
            new AutoRegisterImplementations(builder).ForTypesInAllAssemblies().Register();

            return builder;
        }
    }
}