using System.Reflection;
using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Skeletor.Infrastructure;

[PublicAPI]
public interface IViewAttributeInitializer
{
    void InitFromAssembly(Assembly assembly);

    void InitFromAllAssemblies();
}
