using JetBrains.Annotations;

namespace CreativeCoders.Scripting
{
    [PublicAPI]
    public interface IScriptEngine
    {
        IScriptSession CreateSession(string nameSpace);

        IScriptLanguage Language { get; }
    }
}