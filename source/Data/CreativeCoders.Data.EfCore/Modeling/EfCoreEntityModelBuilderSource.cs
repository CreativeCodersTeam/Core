using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CreativeCoders.Core;
using CreativeCoders.Di;
using CreativeCoders.Core.Reflection;
using JetBrains.Annotations;

namespace CreativeCoders.Data.EfCore.Modeling
{
    [PublicAPI]
    public class EfCoreEntityModelBuilderSource : IEfCoreEntityModelBuilderSource
    {
        private readonly IDiContainer _diContainer;

        private readonly IList<IEfCoreEntityModelBuilder> _builders;

        public EfCoreEntityModelBuilderSource(IDiContainer diContainer)
        {
            _diContainer = diContainer;
            _builders = new List<IEfCoreEntityModelBuilder>();
        }

        public IEnumerable<IEfCoreEntityModelBuilder> GetEntityModelBuilders()
        {
            return _builders;
        }

        public IEfCoreEntityModelBuilderSource Add(Type entityModelBuilderType)
        {
            return !entityModelBuilderType.IsAbstract && entityModelBuilderType.GetInterfaces().Contains(typeof(IEfCoreEntityModelBuilder))
                ? Add(CreateEntityModelBuilder(entityModelBuilderType))
                : this;
        }

        private IEfCoreEntityModelBuilder CreateEntityModelBuilder(Type entityModelBuilderType)
        {
            var entityModelBuilder = _diContainer?.GetInstance(entityModelBuilderType)
                                     ?? Activator.CreateInstance(entityModelBuilderType);

            return (IEfCoreEntityModelBuilder) entityModelBuilder;
        }

        public IEfCoreEntityModelBuilderSource Add(Assembly assembly)
        {
            assembly.GetTypesSafe().ForEach(type => Add(type));

            return this;
        }

        public IEfCoreEntityModelBuilderSource Add(IEfCoreEntityModelBuilder entityModelBuilder)
        {
            if (!_builders.Contains(entityModelBuilder))
            {
                _builders.Add(entityModelBuilder);
            }

            return this;
        }

        public void AddFromAllAssemblies()
        {
            ReflectionUtils
                .GetAllAssemblies()
                .ForEach(assembly => Add(assembly));
        }
    }
}