using JetBrains.Annotations;

namespace CreativeCoders.Scripting
{
    [PublicAPI]
    public interface IScriptSession
    {
        void Execute(IScript script);

        T CreateScriptObject<T>(IScript script) where T: class;

        string NameSpace { get; }
    }
}