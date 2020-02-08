using JetBrains.Annotations;

namespace CreativeCoders.Scripting
{
    [PublicAPI]
    public interface IScriptExecutor
    {
        void Execute(IScript script, IScriptSession session);
    }
}