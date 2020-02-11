using System;
using CreativeCoders.CodeCompilation;
using CreativeCoders.Core;
using CreativeCoders.Scripting.ClassTemplating;
using JetBrains.Annotations;

namespace CreativeCoders.Scripting.Impl
{
    [PublicAPI]
    public class ScriptLanguage : IScriptLanguage
    {
        private readonly ICompilerFactory _compilerFactory;

        public ScriptLanguage(string name, bool supportsDirectExecute,
            ScriptClassTemplate scriptClassTemplate,
            Func<ICompiler> createCompiler) : this(name, supportsDirectExecute, scriptClassTemplate,
            new CompilerFactory(createCompiler)) { }

        public ScriptLanguage(string name, bool supportsDirectExecute,
            ScriptClassTemplate scriptClassTemplate,
            ICompilerFactory compilerFactory)
        {
            Ensure.IsNotNullOrWhitespace(name, nameof(name));
            Ensure.IsNotNull(scriptClassTemplate, nameof(scriptClassTemplate));
            Ensure.IsNotNull(compilerFactory, nameof(compilerFactory));

            ScriptClassTemplate = scriptClassTemplate;
            _compilerFactory = compilerFactory;
            Name = name;
            SupportsDirectExecute = supportsDirectExecute;
        }

        public ICompiler CreateCompiler()
        {
            return _compilerFactory.CreateCompiler();
        }

        public string Name { get; }

        public bool SupportsDirectExecute { get; }

        public string DirectExecuteMethodName { get; set; }

        public ScriptClassTemplate ScriptClassTemplate { get; }
    }
}