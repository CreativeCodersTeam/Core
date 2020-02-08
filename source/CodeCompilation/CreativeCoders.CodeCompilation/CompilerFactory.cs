using System;
using CreativeCoders.Core;

namespace CreativeCoders.CodeCompilation
{
    public class CompilerFactory : ICompilerFactory
    {
        private readonly Func<ICompiler> _createCompilerFunc;

        public CompilerFactory(Func<ICompiler> createCompilerFunc)
        {
            Ensure.IsNotNull(createCompilerFunc, nameof(createCompilerFunc));

            _createCompilerFunc = createCompilerFunc;
        }

        public ICompiler CreateCompiler()
        {
            return _createCompilerFunc();
        }
    }
}