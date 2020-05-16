using System;
using CreativeCoders.Core;
using JetBrains.Annotations;

namespace CreativeCoders.CodeCompilation
{
    [PublicAPI]
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