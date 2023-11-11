using System.Collections;
using System.Collections.Generic;
using CreativeCoders.Core;
using JetBrains.Annotations;

namespace CreativeCoders.Scripting.CSharp.ClassTemplating;

[PublicAPI]
public class ScriptClassMembers : IEnumerable<ScriptClassMember>
{
    private readonly List<ScriptClassMember> _members = new();

    public ScriptClassMethod AddMethod(string methodName, string methodSourceCode)
    {
        Ensure.IsNotNullOrWhitespace(methodName, nameof(methodName));
        Ensure.IsNotNull(methodSourceCode, nameof(methodSourceCode));

        var method = new ScriptClassMethod(methodName, methodSourceCode);
        _members.Add(method);
        return method;
    }

    public ScriptClassProperty AddProperty(string propertyName, string valueType)
    {
        return AddProperty(propertyName, valueType, string.Empty, string.Empty);
    }

    public ScriptClassProperty AddProperty(string propertyName, string valueType, string getterSourceCode)
    {
        return AddProperty(propertyName, valueType, getterSourceCode, string.Empty);
    }

    public ScriptClassProperty AddProperty(string propertyName, string valueType, string getterSourceCode,
        string setterSourceCode)
    {
        Ensure.IsNotNullOrWhitespace(propertyName, nameof(propertyName));
        Ensure.IsNotNullOrWhitespace(valueType, nameof(valueType));

        var property = new ScriptClassProperty(propertyName, valueType, getterSourceCode, setterSourceCode);
        _members.Add(property);
        return property;
    }

    public void AddRawContent(string rawContent)
    {
        var raw = new ScriptClassRawContent(rawContent);
        _members.Add(raw);
    }

    public IEnumerator<ScriptClassMember> GetEnumerator()
    {
        return _members.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
