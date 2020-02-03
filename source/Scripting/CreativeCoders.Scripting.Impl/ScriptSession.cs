using System;
using System.Reflection;
using CreativeCoders.Core;
using CreativeCoders.Core.Logging;
using CreativeCoders.Scripting.Exceptions;
using CreativeCoders.Scripting.Impl.SourceCodeGenerator;

namespace CreativeCoders.Scripting.Impl {
    public class ScriptSession : IScriptSession
    {
        private static readonly ILogger Log = LogManager.GetLogger<ScriptSession>();

        private readonly IScriptLanguage _language;

        public ScriptSession(IScriptEngine scriptEngine, string nameSpace)
        {
            Ensure.IsNotNull(scriptEngine, nameof(scriptEngine));
            Ensure.IsNotNullOrWhitespace(nameSpace, nameof(nameSpace));
            
            _language = scriptEngine.Language;
            NameSpace = nameSpace;
        }

        public void Execute(IScript script)
        {
            Ensure.IsNotNull(script, nameof(script));

            Log.Debug("Execute script");

            var scriptObject = CreateScriptObject<object>(script);
            if (scriptObject == null)
            {
                Log.Error("Script object not found");
                throw new ScriptObjectNotFoundException(typeof(object));
            }
            
            var methodName = _language.DirectExecuteMethodName;
            var method = scriptObject.GetType().GetMethod(methodName);
            if (method == null)
            {
                Log.Error("Script entry point not found");
                throw new ScriptEntryPointNotFoundException(methodName);
            }
            
            method.Invoke(scriptObject, new object[0]);
        }

        public T CreateScriptObject<T>(IScript script) where T: class
        {
            Ensure.IsNotNull(script, nameof(script));

            var template = _language.ScriptClassTemplate;
            var sourceCodeGenerator = new ScriptClassSourceGenerator(template, script, NameSpace);
            var classSourceCode = sourceCodeGenerator.Generate();
            
            var scriptCompiler = new ScriptCompiler(classSourceCode, _language.CreateCompiler());
            var scriptCode = scriptCompiler.CreateAssembly();

            try
            {
                var assembly = Assembly.Load(scriptCode);
                var scriptObject = assembly.CreateInstance(classSourceCode.NameSpace + "." + classSourceCode.ClassName);
                _language.ScriptClassTemplate.Injections.SetupScriptObject(scriptObject);
                return scriptObject as T;
            }
            catch (BadImageFormatException ex)
            {
                Log.Error("Script assembly code has wrong format", ex);
                throw;
            }
        }

        public string NameSpace { get; }
    }
}