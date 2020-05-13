using CreativeCoders.Scripting.Base;

namespace CreativeCoders.Scripting.CSharp
{
    public class CSharpScriptRuntimeSpace<TRuntimeImplementation> : IScriptRuntimeSpace
        where TRuntimeImplementation : CSharpScriptImplementation
    {
        private readonly string _nameSpace;
        
        private readonly TRuntimeImplementation _runtimeImplementation;

        public CSharpScriptRuntimeSpace(string nameSpace, TRuntimeImplementation runtimeImplementation)
        {
            _nameSpace = nameSpace;
            _runtimeImplementation = runtimeImplementation;
        }
        
        public IScript Build(ScriptPackage scriptPackage)
        {
            return _runtimeImplementation.Build(scriptPackage, _nameSpace);
        }
    }
}