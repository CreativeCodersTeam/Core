using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CreativeCoders.Core;
using CreativeCoders.Core.Reflection;
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

        public static IDiContainerBuilder AddTransientCollectionFor<TService>(this IDiContainerBuilder builder)
            where TService : class
        {
            AddCollection(typeof(TService), types => builder.AddTransientCollection<TService>(types));

            return builder;
        }
        
        public static IDiContainerBuilder AddScopedCollectionFor<TService>(this IDiContainerBuilder builder)
            where TService : class
        {
            AddCollection(typeof(TService), types => builder.AddScopedCollection<TService>(types));

            return builder;
        }
        
        public static IDiContainerBuilder AddSingletonCollectionFor<TService>(this IDiContainerBuilder builder)
            where TService : class
        {
            AddCollection(typeof(TService), types => builder.AddScopedCollection<TService>(types));

            return builder;
        }

        private static void AddCollection(Type serviceType, Action<Type[]> addImplementations)
        {
            if (!serviceType.IsInterface)
            {
                throw new NotSupportedException($"Service type must be an interface. Given service type = '{serviceType.FullName}'");
            }

            var implementationTypes = serviceType.GetImplementations().ToArray();
            
            addImplementations(implementationTypes);
        }
    }
}