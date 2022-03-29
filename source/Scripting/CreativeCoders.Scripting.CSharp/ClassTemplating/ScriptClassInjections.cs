using System;
using System.Collections.Generic;
using CreativeCoders.Core;
using CreativeCoders.Core.Collections;

namespace CreativeCoders.Scripting.CSharp.ClassTemplating;

public class ScriptClassInjections
{
    private readonly ScriptClassTemplate _template;

    private readonly IList<IScriptClassInjection> _injections;

    public ScriptClassInjections(ScriptClassTemplate template)
    {
        Ensure.IsNotNull(template, nameof(template));

        _template = template;
        _injections = new List<IScriptClassInjection>();
    }

    public void AddProperty<T>(string propertyName, Func<T> getInjectionData)
    {
        Ensure.IsNotNullOrWhitespace(propertyName, nameof(propertyName));
        Ensure.IsNotNull(getInjectionData, nameof(getInjectionData));

        var propertyInjection = new PropertyInjection<T>(propertyName, getInjectionData);
        _injections.Add(propertyInjection);
        _template.Members.AddProperty(propertyName, typeof(T).Name);
    }

    public void SetupScriptObject(object scriptObject)
    {
        Ensure.IsNotNull(scriptObject, nameof(scriptObject));

        _injections.ForEach(injection => injection.Inject(scriptObject));
    }
}