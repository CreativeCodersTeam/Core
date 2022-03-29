using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CreativeCoders.Scripting.Base;
using CreativeCoders.Scripting.CSharp.ClassTemplating;

namespace CreativeCoders.Scripting.CSharp;

public class CSharpScript : IScript
{
    private readonly Assembly _scriptAssembly;
        
    private readonly string _nameSpace;
        
    private readonly string _className;
        
    private readonly ScriptClassInjections _classTemplateInjections;

    private readonly List<string> _methodNames;

    public CSharpScript(Assembly scriptAssembly, string nameSpace, string className,
        ScriptClassInjections classTemplateInjections)
    {
        _scriptAssembly = scriptAssembly;
        _nameSpace = nameSpace;
        _className = className;
        _classTemplateInjections = classTemplateInjections;
            
        _methodNames = new List<string>();
        FillMethodNames();
    }

    private void FillMethodNames()
    {
        var scriptObjectType = _scriptAssembly.GetType($"{_nameSpace}.{_className}");

        if (scriptObjectType == null)
        {
            return;
        }
            
        var methods = scriptObjectType.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly);
            
        _methodNames.AddRange(
            methods
                .Where(x => x.MemberType == MemberTypes.Method && !x.IsSpecialName)
                .Select(x => x.Name));
    }

    public T CreateObject<T>(IScriptContext scriptContext)
        where T : class
    {
        var scriptObject =  CreateObject<T>();
            
        scriptContext?.SetupScriptObject(scriptObject);

        return scriptObject;
    }

    public T CreateObject<T>()
        where T : class
    {
        var scriptObject = _scriptAssembly.CreateInstance($"{_nameSpace}.{_className}");

        _classTemplateInjections.SetupScriptObject(scriptObject);
            
        return scriptObject as T;
    }

    public IReadOnlyCollection<string> MethodNames => _methodNames;
}