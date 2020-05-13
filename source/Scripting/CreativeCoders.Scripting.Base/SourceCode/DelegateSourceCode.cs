using System;
using CreativeCoders.Core;
using JetBrains.Annotations;

namespace CreativeCoders.Scripting.Base.SourceCode
{
    [PublicAPI]
    public class DelegateSourceCode : ISourceCode
    {
        private readonly Func<string> _getSourceCode;

        public DelegateSourceCode(Func<string> getSourceCode)
        {
            Ensure.IsNotNull(getSourceCode, nameof(getSourceCode));
            
            _getSourceCode = getSourceCode;
        }
        
        public string Read()
        {
            return _getSourceCode();
        }
    }
}