using CreativeCoders.Scripting.Base;

namespace CreativeCoders.Scripting.CSharp
{
    public class CSharpScriptRuntime<TRuntimeImplementation> : IScriptRuntime
        where TRuntimeImplementation : CSharpScriptImplementation
    {
        private readonly TRuntimeImplementation _runtimeImplementation;

        public CSharpScriptRuntime(TRuntimeImplementation runtimeImplementation)
        {
            _runtimeImplementation = runtimeImplementation;
        }
        
        public IScriptRuntimeSpace CreateSpace(string nameSpace)
        {
            return new CSharpScriptRuntimeSpace<TRuntimeImplementation>(nameSpace, _runtimeImplementation);
        }
    }
}