using System;
using CreativeCoders.Core;

namespace CreativeCoders.CodeCompilation
{
    public class CompilerFactory : ICompilerFactory
    {
        private readonly Func<ICompiler> _createCompiler;

        public CompilerFactory(Func<ICompiler> createCompiler)
        {
            Ensure.IsNotNull(createCompiler, nameof(createCompiler));

            _createCompiler = createCompiler;
        }

        public ICompiler CreateCompiler()
        {
            return _createCompiler();
        }
    }
}