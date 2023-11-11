using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CreativeCoders.Core;
using CreativeCoders.Net.XmlRpc.Definition;
using JetBrains.Annotations;

namespace CreativeCoders.Net.XmlRpc.Server;

[PublicAPI]
public class XmlRpcServerMethods : IXmlRpcServerMethods
{
    private readonly List<MethodRegistration> _methods = new();

    public void RegisterMethods<T>(string methodSuffix, T methodsInterface)
        where T : class
    {
        Ensure.IsNotNull(methodsInterface, nameof(methodsInterface));

        var methods =
            typeof(T).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (var methodInfo in methods)
        {
            var methodAttribute = methodInfo.GetCustomAttribute<XmlRpcMethodAttribute>();

            RegisterMethod(methodAttribute, methodInfo, methodSuffix, methodsInterface);
        }
    }

    private void RegisterMethod<T>(XmlRpcMethodAttribute methodAttribute, MethodInfo methodInfo,
        string methodSuffix, T methodsInterface)
    {
        if (methodAttribute == null)
        {
            return;
        }

        var methodName = string.IsNullOrEmpty(methodAttribute.MethodName)
            ? methodInfo.Name
            : methodAttribute.MethodName;
        if (!string.IsNullOrWhiteSpace(methodSuffix))
        {
            methodName = methodSuffix + "." + methodName;
        }

        var methodRegistration = new MethodRegistration(methodName, methodInfo, methodsInterface);
        _methods.Add(methodRegistration);
    }

    public void RegisterMethods<T>(T methodsInterface)
        where T : class
    {
        RegisterMethods(string.Empty, methodsInterface);
    }

    public MethodRegistration GetMethod(string methodName)
    {
        return _methods.FirstOrDefault(m => m.MethodName == methodName);
    }

    public IEnumerable<MethodRegistration> Methods => _methods;
}
