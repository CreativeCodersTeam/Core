using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CreativeCoders.Core;
using CreativeCoders.Core.Collections;
using CreativeCoders.Core.Reflection;
using JetBrains.Annotations;

namespace CreativeCoders.Config.Base
{
    [PublicAPI]
    public static class ConfigurationExtensions
    {
        public static void Initialize(this IConfiguration configuration, IConfigurationInitializer configurationInitializer)
        {
            configurationInitializer.Configure(configuration);
        }

        public static void Initialize(this IConfiguration configuration, IEnumerable<IConfigurationInitializer> configurationInitializers)
        {
            configurationInitializers.ForEach(configuration.Initialize);
        }

        public static void InitializeFromAssembly(this IConfiguration configuration, Assembly assembly, params object[] args)
        {
            assembly.GetTypesSafe()
                .Where(x => !x.IsAbstract && x.GetInterfaces().Contains(typeof(IConfigurationInitializer)))
                .Select(configurationInitializerType => Activator.CreateInstance(configurationInitializerType, args))
                .Cast<IConfigurationInitializer>()
                .WhereNotNull()
                .ForEach(configuration.Initialize);
        }
        
        public static void InitializeFromAssemblies(this IConfiguration configuration, IEnumerable<Assembly> assemblies, params object[] args)
        {
            assemblies.ForEach(assembly => configuration.InitializeFromAssembly(assembly, args));
        }

        public static void InitializeFromAllAssemblies(this IConfiguration configuration, params object[] args)
        {
            configuration.InitializeFromAssemblies(ReflectionUtils.GetAllAssemblies(), args);
        }
    }
}
