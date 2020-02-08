using CreativeCoders.Core;
using CreativeCoders.Scripting.ClassTemplating;

namespace CreativeCoders.Scripting.Impl.SourceCodeGenerator
{
    internal class ScriptClassSourceGenerator
    {
        private readonly ScriptClassTemplate _template;

        private readonly IScript _script;

        private readonly string _nameSpace;

        public ScriptClassSourceGenerator(ScriptClassTemplate template, IScript script, string nameSpace)
        {
            Ensure.IsNotNull(template, nameof(template));
            Ensure.IsNotNull(script, nameof(script));
            Ensure.IsNotNullOrWhitespace(nameSpace, nameof(nameSpace));

            _template = template;
            _script = script;
            _nameSpace = nameSpace;
        }

        public ScriptClassSourceCode Generate()
        {
            var syntaxTree = new ClassSyntaxTreeBuilder(_template, _nameSpace, _script.Name, _script.Usings).Build();
            var code = syntaxTree.Emit(_script);
            var result = new ScriptClassSourceCode(_script, _nameSpace, _script.Name, code);
            return result;
        }
    }
}