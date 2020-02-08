using System.Reflection;
using JetBrains.Annotations;

namespace CreativeCoders.Scripting
{
    [PublicAPI]
    public interface IScriptAssemblyCache
    {
        Assembly LoadAssembly(string scriptId);

        //void StoreAssembly(string scriptId, );
    }
}