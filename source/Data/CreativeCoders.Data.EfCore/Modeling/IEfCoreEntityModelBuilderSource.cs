using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;

namespace CreativeCoders.Data.EfCore.Modeling
{
    [PublicAPI]
    public interface IEfCoreEntityModelBuilderSource
    {
        IEfCoreEntityModelBuilderSource Add(Type entityModelBuilderType);

        IEfCoreEntityModelBuilderSource Add(Assembly assembly);

        IEfCoreEntityModelBuilderSource Add(IEfCoreEntityModelBuilder entityModelBuilder);

        void AddFromAllAssemblies();

        IEnumerable<IEfCoreEntityModelBuilder> GetEntityModelBuilders();
    }
}