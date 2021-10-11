using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CreativeCoders.Core.Collections;
using CreativeCoders.Core.Reflection;

namespace CreativeCoders.Di.Building
{
    public class AutoRegisterImplementations
    {
        private readonly IDiContainerBuilder _containerBuilder;

        private readonly IList<Type> _types;

        public AutoRegisterImplementations(IDiContainerBuilder containerBuilder)
        {
            _containerBuilder = containerBuilder;
            _types = new List<Type>();
        }

        public AutoRegisterImplementations ForTypesInAllAssemblies()
        {
            var types = ReflectionUtils.GetAllTypes(assembly => !assembly.IsDynamic);
            
            return ForTypes(types);
        }

        public AutoRegisterImplementations ForTypesInAssembly(Assembly assembly)
        {
            var types = assembly.GetTypes();

            return ForTypes(types);
        }

        public AutoRegisterImplementations ForTypes(IEnumerable<Type> types)
        {
            types.ForEach(_types.Add);

            return this;
        }

        public void Register()
        {
            foreach (var type in _types.Distinct())
            {
                if (type
                    .GetCustomAttributes(typeof(ImplementsAttribute), false)
                    .FirstOrDefault() is ImplementsAttribute implementsAttribute)
                {
                    RegisterImplementation(type, implementsAttribute.ServiceType, implementsAttribute.Lifecycle);
                }
            }
        }

        private void RegisterImplementation(Type implementationType, Type serviceType, ImplementationLifecycle lifecycle)
        {
            switch (lifecycle)
            {
                case ImplementationLifecycle.Transient:
                    _containerBuilder.AddTransient(serviceType, implementationType);
                    break;
                case ImplementationLifecycle.Scoped:
                    _containerBuilder.AddScoped(serviceType, implementationType);
                    break;
                case ImplementationLifecycle.Singleton:
                    _containerBuilder.AddSingleton(serviceType, implementationType);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lifecycle), lifecycle, null);
            }
        }
    }
}
